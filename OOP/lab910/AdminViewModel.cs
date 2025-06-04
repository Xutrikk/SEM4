using lab4wpf5oop.Models;
using RouteBookingSystem.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.EntityFrameworkCore;

namespace RouteBookingSystem
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private readonly IUnitOfWork _unitOfWork;
        private ObservableCollection<Ticket> tickets;
        private ObservableCollection<User> users;
        private ObservableCollection<PurchasedTicket> purchasedTickets;
        private User selectedUser;
        private Ticket selectedTicket;
        private PurchasedTicket selectedPurchasedTicket;
        private string _ticketSearchQuery;
        private double _minPrice;
        private double _maxPrice = 60;
        private string _userSearchQuery;
        private bool _showAdminsOnly;
        private string _selectedUserPassword;
        private readonly User currentAdmin;
        private bool _isNewUser;
        private string _routeStatistics;
        private SeriesCollection _routeStatisticsSeries;
        private string _purchasedTicketSearchQuery;
        private bool? _userFilterIsBlocked;
        private int? _purchasedTicketStatusFilter;
        private DateTime? _ticketDateFilter;

        private readonly Dictionary<string, string> _transportTypeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Bus"] = "Автобус",
            ["Marshrutka"] = "Маршрутка",
            ["Автобус"] = "Автобус",
            ["Маршрутка"] = "Маршрутка",
        };

        public bool IsNewUser
        {
            get => _isNewUser;
            set { _isNewUser = value; OnPropertyChanged(); }
        }

        public string SelectedUserPassword
        {
            get => _selectedUserPassword;
            set { _selectedUserPassword = value; OnPropertyChanged(); }
        }

        public ObservableCollection<PurchasedTicket> PurchasedTickets
        {
            get => purchasedTickets;
            set
            {
                purchasedTickets = value;
                OnPropertyChanged();
                UpdateRouteStatistics();
            }
        }

        public PurchasedTicket SelectedPurchasedTicket
        {
            get => selectedPurchasedTicket;
            set
            {
                selectedPurchasedTicket = value;
                OnPropertyChanged();
            }
        }

        public string RouteStatistics
        {
            get => _routeStatistics;
            set { _routeStatistics = value; OnPropertyChanged(); }
        }

        public SeriesCollection RouteStatisticsSeries
        {
            get => _routeStatisticsSeries;
            set { _routeStatisticsSeries = value; OnPropertyChanged(); }
        }

        public string PurchasedTicketSearchQuery
        {
            get => _purchasedTicketSearchQuery;
            set { _purchasedTicketSearchQuery = value; OnPropertyChanged(); UpdateFilteredPurchasedTickets(); }
        }

        public bool? UserFilterIsBlocked
        {
            get => _userFilterIsBlocked;
            set { _userFilterIsBlocked = value; OnPropertyChanged(); UpdateFilteredUsers(); }
        }

        public int? PurchasedTicketStatusFilter
        {
            get => _purchasedTicketStatusFilter;
            set { _purchasedTicketStatusFilter = value; OnPropertyChanged(); UpdateFilteredPurchasedTickets(); }
        }

        public DateTime? TicketDateFilter
        {
            get => _ticketDateFilter;
            set { _ticketDateFilter = value; OnPropertyChanged(); UpdateFilteredTickets(); }
        }

        public AdminViewModel(IUnitOfWork unitOfWork, User currentAdmin)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.currentAdmin = currentAdmin ?? throw new ArgumentNullException(nameof(currentAdmin));
            Tickets = new ObservableCollection<Ticket>();
            Users = new ObservableCollection<User>();
            PurchasedTickets = new ObservableCollection<PurchasedTicket>();
            FilteredTickets = new ObservableCollection<Ticket>();
            FilteredUsers = new ObservableCollection<User>();
            FilteredPurchasedTickets = new ObservableCollection<PurchasedTicket>();
        }

        private void UpdateRouteStatistics()
        {
            var routeStats = PurchasedTickets
                .GroupBy(pt => $"{pt.From}-{pt.To}")
                .Select(g => new
                {
                    Route = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(g => g.Count)
                .ThenBy(g => g.Route)
                .ToList();

            int totalOrders = PurchasedTickets.Count;
            if (totalOrders == 0)
            {
                RouteStatistics = "Нет заказов для отображения статистики.";
                RouteStatisticsSeries = new SeriesCollection();
                return;
            }

            var pieSeries = new SeriesCollection();
            foreach (var stat in routeStats)
            {
                double percentage = (double)stat.Count / totalOrders * 100;
                pieSeries.Add(new PieSeries
                {
                    Title = stat.Route,
                    Values = new ChartValues<double> { stat.Count },
                    DataLabels = true
                });
            }

            RouteStatistics = "";
            RouteStatisticsSeries = pieSeries;
        }

        private string MapTransportType(string inputType)
        {
            return _transportTypeMap.TryGetValue(inputType, out var result)
                ? result
                : inputType;
        }

        private string MapTransportTypeForFilter(string inputKey)
        {
            if (inputKey == "AllTypes") return null;
            return _transportTypeMap.TryGetValue(inputKey, out var value) ? value : null;
        }

        private bool ValidateUser(User user, string password, bool requirePassword, out string errorMessage)
        {
            var errors = new StringBuilder();
            CheckField(user.FirstName, "Имя", errors);
            CheckField(user.LastName, "Фамилия", errors);
            CheckField(user.Surname, "Отчество", errors);
            CheckField(user.Email, "Email", errors);
            if (requirePassword)
            {
                CheckField(password, "Пароль", errors);
            }
            CheckField(user.PhoneNumber, "Номер телефона", errors);

            if (!string.IsNullOrEmpty(user.FirstName) && !Regex.IsMatch(user.FirstName, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("• Имя должно содержать только русские или английские буквы");
            }
            if (!string.IsNullOrEmpty(user.LastName) && !Regex.IsMatch(user.LastName, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("• Фамилия должна содержать только русские или английские буквы");
            }
            if (!string.IsNullOrEmpty(user.Surname) && !Regex.IsMatch(user.Surname, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("• Отчество должно содержать только русские или английские буквы");
            }
            if (!IsValidEmail(user.Email))
            {
                errors.AppendLine("• Неверный формат Email (только английские буквы и цифры, без спецсимволов (кроме @ и .), без пробелов)");
            }
            if (!IsValidPhone(user.PhoneNumber))
            {
                errors.AppendLine("• Телефон должен быть в формате: +375XXXXXXXXX");
            }
            if (requirePassword && !string.IsNullOrEmpty(password) && !IsValidPassword(password))
            {
                errors.AppendLine("• Пароль должен содержать минимум 6 символов, только английские буквы, цифры и спецсимволы, без пробелов");
            }

            errorMessage = errors.ToString();
            return errorMessage.Length == 0;
        }

        private bool ValidateTicket(Ticket ticket, out string errorMessage)
        {
            var errors = new StringBuilder();
            bool isValid = true;

            if (ticket == null)
            {
                errors.AppendLine("• Маршрут не выбран");
                isValid = false;
            }
            else
            {
                CheckField(ticket.From, "Откуда", errors);
                CheckField(ticket.To, "Куда", errors);
                CheckField(ticket.Date.ToString(), "Дата", errors);
                CheckField(ticket.Type, "Тип транспорта", errors);
                CheckField(ticket.Description, "Описание", errors);
                CheckField(ticket.BoardingPoints, "Место посадки", errors);
                CheckField(ticket.DropOffPoints, "Место высадки", errors);

                if (string.IsNullOrWhiteSpace(ticket.Time.ToString(@"hh\:mm")))
                {
                    errors.AppendLine("• Время должно быть указано в формате hh:mm");
                    isValid = false;
                }

                if (ticket.Date < DateTime.Today)
                {
                    errors.AppendLine("• Дата маршрута не может быть раньше текущей даты");
                    isValid = false;
                }

                if (ticket.Price <= 0 || !Regex.IsMatch(ticket.Price.ToString(System.Globalization.CultureInfo.InvariantCulture), @"^\d+\.?\d*$"))
                {
                    errors.AppendLine("• Цена должна быть положительным числом");
                    isValid = false;
                }

                if (ticket.Number <= 0)
                {
                    errors.AppendLine("• Количество должно быть целым числом больше 0");
                    isValid = false;
                }
                else if (ticket.Date.Date == DateTime.Today)
                {
                    TimeSpan currentTime = DateTime.Now.TimeOfDay;
                    TimeSpan minAllowedTime = currentTime.Add(TimeSpan.FromHours(8));
                    if (minAllowedTime >= TimeSpan.FromHours(24))
                    {
                        errors.AppendLine("• Сегодня уже нельзя добавить маршрут — текущее время не позволяет!");
                        isValid = false;
                    }
                    else if (ticket.Time < minAllowedTime)
                    {
                        errors.AppendLine($"• Время для сегодняшней даты должно быть не ранее {minAllowedTime.Hours:D2}:{minAllowedTime.Minutes:D2}");
                        isValid = false;
                    }
                }

                if (!string.IsNullOrEmpty(ticket.From) && !Regex.IsMatch(ticket.From, @"^[а-яА-ЯёЁa-zA-Z\s-,]+$"))
                {
                    errors.AppendLine("• Поле 'Откуда' должно содержать только русские или английские буквы, пробелы, дефисы, запятые");
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(ticket.To) && !Regex.IsMatch(ticket.To, @"^[а-яА-ЯёЁa-zA-Z\s-,]+$"))
                {
                    errors.AppendLine("• Поле 'Куда' должно содержать только русские или английские буквы, пробелы, дефисы, запятые");
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(ticket.BoardingPoints) &&
                    !Regex.IsMatch(ticket.BoardingPoints, @"^[а-яА-ЯёЁa-zA-Z\s-,]*(?:[а-яА-ЯёЁa-zA-Z]+[0-9]+|[0-9]+[а-яА-ЯёЁa-zA-Z]+)[а-яА-ЯёЁa-zA-Z\s-,]*$|^[а-яА-ЯёЁa-zA-Z\s-,]+$"))
                {
                    errors.AppendLine("• Поле 'Место посадки' должно содержать только русские или английские буквы, числа (в сочетании с буквами), пробелы, дефисы, запятые");
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(ticket.DropOffPoints) &&
                    !Regex.IsMatch(ticket.DropOffPoints, @"^[а-яА-ЯёЁa-zA-Z\s-,]*(?:[а-яА-ЯёЁa-zA-Z]+[0-9]+|[0-9]+[а-яА-ЯёЁa-zA-Z]+)[а-яА-ЯёЁa-zA-Z\s-,]*$|^[а-яА-ЯёЁa-zA-Z\s-,]+$"))
                {
                    errors.AppendLine("• Поле 'Место высадки' должно содержать только русские или английские буквы, числа (в сочетании с буквами), пробелы, дефисы, запятые");
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(ticket.Description) && !Regex.IsMatch(ticket.Description, @"^[а-яА-ЯёЁa-zA-Z\s-,]+$"))
                {
                    errors.AppendLine("• Поле 'Описание' должно содержать только русские или английские буквы, пробелы, дефисы, запятые");
                    isValid = false;
                }
                if (!string.IsNullOrEmpty(ticket.Company) && !Regex.IsMatch(ticket.Company, @"^[а-яА-ЯёЁa-zA-Z\s-,]+$"))
                {
                    errors.AppendLine("• Поле 'Компания' должно содержать только русские или английские буквы, пробелы, дефисы, запятые");
                    isValid = false;
                }
            }

            errorMessage = errors.ToString();
            return isValid && errors.Length == 0;
        }

        private void CheckField(string value, string fieldName, StringBuilder errors)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.AppendLine($"• Поле '{fieldName}' обязательно для заполнения");
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z][a-zA-Z0-9]*@[a-zA-Z][a-zA-Z0-9]*\.[a-zA-Z]+$");
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\+375(25|29|33|44)\d{7}$");
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 6 &&
                   Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()+\-=\[\]{};:'"",.<>?]+$") &&
                   !password.Contains(" ");
        }

        public ObservableCollection<Ticket> Tickets
        {
            get => tickets;
            set { tickets = value; OnPropertyChanged(); }
        }

        public ObservableCollection<User> Users
        {
            get => users;
            set { users = value; OnPropertyChanged(); }
        }

        public User SelectedUser
        {
            get => selectedUser;
            set
            {
                if (value != null)
                {
                    selectedUser = new User
                    {
                        FirstName = value.FirstName,
                        LastName = value.LastName,
                        Surname = value.Surname,
                        Email = value.Email,
                        PasswordHash = value.PasswordHash,
                        PhoneNumber = value.PhoneNumber,
                        IsAdmin = value.IsAdmin,
                        IsBlocked = value.IsBlocked
                    };
                }
                else
                {
                    selectedUser = null;
                }
                OnPropertyChanged();
                SelectedUserPassword = "";
                IsNewUser = selectedUser != null && !Users.Any(u => u.Email == selectedUser.Email);
            }
        }

        public Ticket SelectedTicket
        {
            get => selectedTicket;
            set { selectedTicket = value; OnPropertyChanged(); }
        }

        public string TicketSearchQuery
        {
            get => _ticketSearchQuery;
            set { _ticketSearchQuery = value; OnPropertyChanged(); UpdateFilteredTickets(); }
        }

        public double MinPrice
        {
            get => _minPrice;
            set { _minPrice = value; OnPropertyChanged(); UpdateFilteredTickets(); }
        }

        public double MaxPrice
        {
            get => _maxPrice;
            set { _maxPrice = value; OnPropertyChanged(); UpdateFilteredTickets(); }
        }

        public string UserSearchQuery
        {
            get => _userSearchQuery;
            set { _userSearchQuery = value; OnPropertyChanged(); UpdateFilteredUsers(); }
        }

        public bool ShowAdminsOnly
        {
            get => _showAdminsOnly;
            set { _showAdminsOnly = value; OnPropertyChanged(); UpdateFilteredUsers(); }
        }

        private string _selectedTransportType;
        public string SelectedTransportType
        {
            get => _selectedTransportType;
            set
            {
                _selectedTransportType = value;
                OnPropertyChanged();
                UpdateFilteredTickets();
            }
        }

        public ObservableCollection<Ticket> FilteredTickets { get; set; }
        public ObservableCollection<User> FilteredUsers { get; set; }
        public ObservableCollection<PurchasedTicket> FilteredPurchasedTickets { get; set; }

        private ObservableCollection<string> _transportTypesForFilter;
        public ObservableCollection<string> TransportTypesForFilter
        {
            get => _transportTypesForFilter;
            set { _transportTypesForFilter = value; OnPropertyChanged(); }
        }

        private ObservableCollection<string> _transportTypesForEdit;
        public ObservableCollection<string> TransportTypesForEdit
        {
            get => _transportTypesForEdit;
            set { _transportTypesForEdit = value; OnPropertyChanged(); }
        }

        public ICommand DeleteTicketCommand { get; private set; }
        public ICommand SaveTicketCommand { get; private set; }
        public ICommand AddTicketCommand { get; private set; }
        public ICommand DeleteUserCommand { get; private set; }
        public ICommand SaveUserCommand { get; private set; }
        public ICommand AddUserCommand { get; private set; }
        public ICommand DeletePurchasedTicketCommand { get; private set; }
        public ICommand UpdateUserAdminCommand { get; private set; }
        public ICommand UpdateUserBlockedCommand { get; private set; }

        private void InitializeCommands()
        {
            DeleteTicketCommand = new RelayCommand(_ => DeleteTicket());
            SaveTicketCommand = new RelayCommand(_ => SaveTicket());
            AddTicketCommand = new RelayCommand(_ => AddTicket());
            DeleteUserCommand = new RelayCommand(_ => DeleteUser());
            SaveUserCommand = new RelayCommand(_ => SaveUser());
            AddUserCommand = new RelayCommand(_ => AddUser());
            DeletePurchasedTicketCommand = new RelayCommand(_ => DeletePurchasedTicket());
            UpdateUserAdminCommand = new RelayCommand(_ => UpdateUserAdmin());
            UpdateUserBlockedCommand = new RelayCommand(_ => UpdateUserBlocked());
        }

        private async void UpdateUserAdmin()
        {
            if (SelectedUser == null) return;
            if (SelectedUser.Email == currentAdmin.Email && !SelectedUser.IsAdmin)
            {
                MessageBox.Show("Вы не можете снять статус администратора с самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                SelectedUser.IsAdmin = true;
                return;
            }

            using var transaction = await ((UnitOfWork)_unitOfWork).Context.Database.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Users.UpdateAsync(SelectedUser);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                UpdateFilteredUsers();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Ошибка обновления статуса администратора: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void UpdateUserBlocked()
        {
            if (SelectedUser == null) return;
            if (SelectedUser.Email == currentAdmin.Email && SelectedUser.IsBlocked)
            {
                MessageBox.Show("Вы не можете заблокировать самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                SelectedUser.IsBlocked = false;
                return;
            }

            using var transaction = await ((UnitOfWork)_unitOfWork).Context.Database.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Users.UpdateAsync(SelectedUser);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                UpdateFilteredUsers();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Ошибка обновления статуса блокировки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task InitializeAsync()
        {
            await LoadData();
            InitializeCommands();
            FilteredTickets = new ObservableCollection<Ticket>(Tickets);
            FilteredUsers = new ObservableCollection<User>(Users);
            FilteredPurchasedTickets = new ObservableCollection<PurchasedTicket>(PurchasedTickets);
            if (FilteredPurchasedTickets.Count > 0) SelectedPurchasedTicket = FilteredPurchasedTickets[0];
            UpdateTransportTypes();
            UpdateRouteStatistics();
        }

        private async Task LoadData()
        {
            Tickets = new ObservableCollection<Ticket>(await LoadTickets());
            Users = new ObservableCollection<User>(await LoadUsers());
            PurchasedTickets = new ObservableCollection<PurchasedTicket>(await LoadPurchasedTickets());
            FilteredUsers = new ObservableCollection<User>(Users);
            FilteredPurchasedTickets = new ObservableCollection<PurchasedTicket>(PurchasedTickets);
            OnPropertyChanged(nameof(FilteredUsers));
            OnPropertyChanged(nameof(FilteredPurchasedTickets));
        }

        public void RefreshTransportTypes()
        {
            UpdateTransportTypes();
            UpdateFilteredTickets();
            OnPropertyChanged(nameof(TransportTypesForFilter));
        }

        private void UpdateTransportTypes()
        {
            TransportTypesForFilter = new ObservableCollection<string>
            {
                Application.Current.Resources["AllTypes"].ToString(),
                Application.Current.Resources["Bus"].ToString(),
                Application.Current.Resources["Marshrutka"].ToString()
            };

            TransportTypesForEdit = new ObservableCollection<string>(
                _transportTypeMap.Values.Distinct().ToList()
            );
            SelectedTransportType = Application.Current.Resources["AllTypes"].ToString();
        }

        public void UpdateFilteredTickets()
        {
            var dbType = MapTransportTypeForFilter(SelectedTransportType);

            var filtered = Tickets
                .Where(t => string.IsNullOrEmpty(TicketSearchQuery) ||
                            t.From.Contains(TicketSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                            t.To.Contains(TicketSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                            t.Company.Contains(TicketSearchQuery, StringComparison.OrdinalIgnoreCase))
                .Where(t => t.Price >= MinPrice && t.Price <= MaxPrice)
                .Where(t => dbType == null || t.Type == dbType)
                .Where(t => !_ticketDateFilter.HasValue || t.Date.Date == _ticketDateFilter.Value.Date)
                .OrderBy(t => t.Price)
                .ThenBy(t => t.Date)
                .ThenBy(t => t.From)
                .ToList();

            FilteredTickets = new ObservableCollection<Ticket>(filtered);
            OnPropertyChanged(nameof(FilteredTickets));
        }

        private void UpdateFilteredUsers()
        {
            var filtered = Users
                .Where(u => string.IsNullOrEmpty(UserSearchQuery) ||
                            u.Email.Contains(UserSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                            u.FirstName.Contains(UserSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                            u.LastName.Contains(UserSearchQuery, StringComparison.OrdinalIgnoreCase))
                .Where(u => !ShowAdminsOnly || u.IsAdmin)
                .Where(u => !_userFilterIsBlocked.HasValue || u.IsBlocked == _userFilterIsBlocked.Value)
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .ToList();

            FilteredUsers = new ObservableCollection<User>(filtered);
            OnPropertyChanged(nameof(FilteredUsers));
        }

        private void UpdateFilteredPurchasedTickets()
        {
            var filtered = PurchasedTickets
                .Where(pt => string.IsNullOrEmpty(PurchasedTicketSearchQuery) ||
                             pt.EmailUser.Contains(PurchasedTicketSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                             pt.From.Contains(PurchasedTicketSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                             pt.To.Contains(PurchasedTicketSearchQuery, StringComparison.OrdinalIgnoreCase))
                .Where(pt => !_purchasedTicketStatusFilter.HasValue || pt.Status == _purchasedTicketStatusFilter.Value)
                .OrderByDescending(pt => pt.PurchaseDate)
                .ThenBy(pt => pt.PurchaseId)
                .ToList();

            FilteredPurchasedTickets = new ObservableCollection<PurchasedTicket>(filtered);
            OnPropertyChanged(nameof(FilteredPurchasedTickets));
        }

        private async void DeleteTicket()
        {
            if (SelectedTicket == null) return;
            if (MessageBox.Show("Удалить этот маршрут?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            using var transaction = await ((UnitOfWork)_unitOfWork).Context.Database.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Tickets.DeleteAsync(SelectedTicket);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                Tickets.Remove(SelectedTicket);
                UpdateFilteredTickets();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveTicket()
        {
            if (SelectedTicket == null)
            {
                MessageBox.Show("Выберите или создайте маршрут!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!ValidateTicket(SelectedTicket, out var error))
            {
                MessageBox.Show(error, "Ошибки ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var transaction = await ((UnitOfWork)_unitOfWork).Context.Database.BeginTransactionAsync();
            try
            {
                if (SelectedTicket.Id == 0)
                {
                    await _unitOfWork.Tickets.AddAsync(SelectedTicket);
                    Tickets.Add(SelectedTicket);
                    MessageBox.Show("Маршрут успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    AddTicket();
                }
                else
                {
                    await _unitOfWork.Tickets.UpdateAsync(SelectedTicket);
                    MessageBox.Show("Маршрут успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                UpdateFilteredTickets();
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Ошибка базы данных: {ex.InnerException?.Message ?? ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Неизвестная ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTicket()
        {
            try
            {
                SelectedTicket = new Ticket
                {
                    Date = DateTime.Today,
                    Time = DateTime.Now.TimeOfDay,
                    Price = 0,
                    Number = 0,
                    Type = TransportTypesForEdit.FirstOrDefault() ?? "Автобус",
                    From = "",
                    To = "",
                    BoardingPoints = "",
                    DropOffPoints = "",
                    Description = "",
                    Company = ""
                };
                OnPropertyChanged(nameof(SelectedTicket));
                UpdateFilteredTickets();
                MessageBox.Show("Новый маршрут создан. Заполните поля и сохраните.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при создании маршрута: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteUser()
        {
            if (SelectedUser == null) return;
            if (SelectedUser.Email == currentAdmin.Email)
            {
                MessageBox.Show("Вы не можете удалить самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (MessageBox.Show("Удалить этого пользователя?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            using var transaction = await ((UnitOfWork)_unitOfWork).Context.Database.BeginTransactionAsync();
            try
            {
                await _unitOfWork.Users.DeleteAsync(SelectedUser);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                Users.Remove(Users.FirstOrDefault(u => u.Email == SelectedUser.Email));
                UpdateFilteredUsers();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeletePurchasedTicket()
        {
            if (SelectedPurchasedTicket == null) return;
            if (MessageBox.Show("Удалить этот заказ?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            using var transaction = await ((UnitOfWork)_unitOfWork).Context.Database.BeginTransactionAsync();
            try
            {
                var ticket = Tickets.FirstOrDefault(t => t.Id == SelectedPurchasedTicket.TicketId);
                if (ticket != null)
                {
                    ticket.Number += SelectedPurchasedTicket.Number;
                    await _unitOfWork.Tickets.UpdateAsync(ticket);
                }
                await _unitOfWork.PurchasedTickets.DeleteAsync(SelectedPurchasedTicket);
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                PurchasedTickets.Remove(SelectedPurchasedTicket);
                FilteredPurchasedTickets.Remove(SelectedPurchasedTicket);
                SelectedPurchasedTicket = null;
                UpdateRouteStatistics();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Ошибка удаления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void SaveUser()
        {
            if (SelectedUser == null) return;
            var tempUser = new User
            {
                FirstName = SelectedUser.FirstName,
                LastName = SelectedUser.LastName,
                Surname = SelectedUser.Surname,
                Email = SelectedUser.Email,
                PasswordHash = SelectedUser.PasswordHash,
                PhoneNumber = SelectedUser.PhoneNumber,
                IsAdmin = SelectedUser.IsAdmin,
                IsBlocked = SelectedUser.IsBlocked
            };

            bool requirePassword = IsNewUser || !string.IsNullOrEmpty(SelectedUserPassword);
            if (!ValidateUser(tempUser, SelectedUserPassword, requirePassword, out var errorMessage))
            {
                MessageBox.Show(errorMessage, "Ошибки ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (tempUser.IsBlocked && tempUser.Email == currentAdmin.Email)
            {
                MessageBox.Show("Вы не можете заблокировать самого себя!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using var transaction = await ((UnitOfWork)_unitOfWork).Context.Database.BeginTransactionAsync();
            try
            {
                if (!string.IsNullOrEmpty(SelectedUserPassword))
                {
                    tempUser.PasswordHash = HashPassword(SelectedUserPassword);
                }
                else if (!IsNewUser)
                {
                    var existingUser = Users.FirstOrDefault(u => u.Email == tempUser.Email);
                    if (existingUser != null)
                    {
                        tempUser.PasswordHash = existingUser.PasswordHash;
                    }
                }

                if (IsNewUser)
                {
                    var existingCount = (await _unitOfWork.Users.FindAsync(u => u.Email == tempUser.Email)).Count();
                    if (existingCount > 0)
                    {
                        MessageBox.Show("Пользователь с таким email уже существует!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        await transaction.RollbackAsync();
                        return;
                    }
                    await _unitOfWork.Users.AddAsync(tempUser);
                    Users.Add(tempUser);
                    MessageBox.Show("Пользователь успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    AddUser();
                }
                else
                {
                    await _unitOfWork.Users.UpdateAsync(tempUser);
                    var existingUser = Users.FirstOrDefault(u => u.Email == tempUser.Email);
                    if (existingUser != null)
                    {
                        existingUser.FirstName = tempUser.FirstName;
                        existingUser.LastName = tempUser.LastName;
                        existingUser.Surname = tempUser.Surname;
                        existingUser.PhoneNumber = tempUser.PhoneNumber;
                        existingUser.IsAdmin = tempUser.IsAdmin;
                        existingUser.IsBlocked = tempUser.IsBlocked;
                        existingUser.PasswordHash = tempUser.PasswordHash;
                    }
                    SelectedUser.FirstName = tempUser.FirstName;
                    SelectedUser.LastName = tempUser.LastName;
                    SelectedUser.Surname = tempUser.Surname;
                    SelectedUser.PhoneNumber = tempUser.PhoneNumber;
                    SelectedUser.IsAdmin = tempUser.IsAdmin;
                    SelectedUser.IsBlocked = tempUser.IsBlocked;
                    SelectedUser.PasswordHash = tempUser.PasswordHash;
                    MessageBox.Show("Пользователь успешно отредактирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                await _unitOfWork.SaveChangesAsync();
                await transaction.CommitAsync();
                UpdateFilteredUsers();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddUser()
        {
            SelectedUser = new User
            {
                FirstName = "",
                LastName = "",
                Surname = "",
                Email = "",
                PasswordHash = "",
                PhoneNumber = "",
                IsAdmin = false,
                IsBlocked = false
            };
            SelectedUserPassword = "";
            IsNewUser = true;
        }

        private async Task<List<Ticket>> LoadTickets()
        {
            var tickets = await _unitOfWork.Tickets.GetAllAsync();
            return tickets.Select(t =>
            {
                var dbType = t.Type;
                var localizedType = _transportTypeMap.FirstOrDefault(x => x.Value == dbType).Key ?? dbType;
                t.Type = Application.Current.Resources[localizedType]?.ToString() ?? dbType;
                return t;
            }).OrderBy(t => t.Price).ThenBy(t => t.Date).ThenBy(t => t.From).ToList();
        }

        private async Task<List<User>> LoadUsers()
        {
            return (await _unitOfWork.Users.GetAllAsync()).OrderBy(u => u.LastName).ThenBy(u => u.FirstName).ToList();
        }

        private async Task<List<PurchasedTicket>> LoadPurchasedTickets()
        {
            var purchasedTickets = await _unitOfWork.PurchasedTickets.GetAllAsync();
            foreach (var ticket in purchasedTickets)
            {
                await LoadRatingAndTicketAsync(ticket);
            }
            return purchasedTickets.OrderByDescending(pt => pt.PurchaseDate).ThenBy(pt => pt.PurchaseId).ToList();
        }

        private async Task LoadRatingAndTicketAsync(PurchasedTicket ticket)
        {
            try
            {
                var rating = (await _unitOfWork.TripRatings.FindAsync(tr =>
                    tr.PurchaseId == ticket.PurchaseId &&
                    tr.EmailUser == ticket.EmailUser))
                    .FirstOrDefault();
                ticket.TripRating = rating;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки рейтинга: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
