using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace RouteBookingSystem
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private readonly string connectionString;
        private ObservableCollection<Ticket> tickets;
        private ObservableCollection<User> users;
        private User selectedUser;
        private Ticket selectedTicket;
        private string _ticketSearchQuery;
        private double _minPrice;
        private double _maxPrice = 60;
        private string _userSearchQuery;
        private bool _showAdminsOnly;

        private readonly Dictionary<string, string> _transportTypeMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["Bus"] = "Автобус",
            ["Marshrutka"] = "Маршрутка",
            ["Автобус"] = "Автобус",
            ["Маршрутка"] = "Маршрутка",
        };
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
        private bool ValidateUser(User user, out string errorMessage)
        {
            var errors = new StringBuilder();

            // Проверка обязательных полей
            CheckField(user.FirstName, "Имя", errors);
            CheckField(user.LastName, "Фамилия", errors);
            CheckField(user.Surname, "Отчество", errors);
            CheckField(user.Email, "Email", errors);
            CheckField(user.PasswordHash, "Пароль", errors);
            CheckField(user.PhoneNumber, "Номер телефона", errors);

            // Валидация форматов
            if (!IsValidEmail(user.Email))
            {
                errors.AppendLine("• Неверный формат Email");
            }

            if (!IsValidPhone(user.PhoneNumber))
            {
                errors.AppendLine("• Телефон должен быть в формате: +375XXXXXXXXX");
            }

            errorMessage = errors.ToString();
            return errorMessage.Length == 0;
        }

        private bool ValidateTicket(Ticket ticket, out string errorMessage)
        {
            var errors = new StringBuilder();
            bool isValid = true;

            // Проверка обязательных полей
            CheckField(ticket.From, "Откуда", errors);
            CheckField(ticket.To, "Куда", errors);
            CheckField(ticket.Date.ToString(), "Дата", errors);
            CheckField(ticket.Time, "Время", errors);
            CheckField(ticket.Type, "Тип транспорта", errors);
            CheckField(ticket.Description, "Описание", errors);

            // Валидация форматов
            if (!TimeSpan.TryParseExact(ticket.Time, @"hh\:mm", CultureInfo.InvariantCulture, out _))
            {
                errors.AppendLine("• Время должно быть в формате HH:mm");
                isValid = false;
            }

            if (ticket.Price <= 0)
                errors.AppendLine("• Цена должна быть больше 0");

            if (ticket.Number <= 0)
                errors.AppendLine("• Количество мест должно быть больше 0");

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
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
        }

        private bool IsValidPhone(string phone)
        {
            return Regex.IsMatch(phone, @"^\+375(25|29|33|44)\d{7}$");
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
            set { selectedUser = value; OnPropertyChanged(); }
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

        public AdminViewModel(string connString)
        {
            connectionString = connString;
            LoadData();
            InitializeCommands();
            FilteredTickets = new ObservableCollection<Ticket>(Tickets);
            FilteredUsers = new ObservableCollection<User>(Users);
            UpdateTransportTypes(); 
        }

        private void InitializeCommands()
        {
            DeleteTicketCommand = new RelayCommand(_ => DeleteTicket());
            SaveTicketCommand = new RelayCommand(_ => SaveTicket());
            AddTicketCommand = new RelayCommand(_ => AddTicket());
            DeleteUserCommand = new RelayCommand(_ => DeleteUser());
            SaveUserCommand = new RelayCommand(_ => SaveUser());
            AddUserCommand = new RelayCommand(_ => AddUser());
        }

        public void LoadData()
        {
            Tickets = new ObservableCollection<Ticket>(LoadTickets());
            Users = new ObservableCollection<User>(LoadUsers());
        }
        public void RefreshTransportTypes()
        {
            UpdateTransportTypes();
            UpdateFilteredTickets();
            OnPropertyChanged(nameof(TransportTypesForFilter));
        }

        private void UpdateTransportTypes()
        {
            // Используем реальные значения из ресурсов
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
                .Where(t => (string.IsNullOrEmpty(TicketSearchQuery) ||
                           t.From.Contains(TicketSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                           t.To.Contains(TicketSearchQuery, StringComparison.OrdinalIgnoreCase)))
                .Where(t => t.Price >= MinPrice && t.Price <= MaxPrice)
                .Where(t => dbType == null || t.Type == dbType)
                .ToList();

            FilteredTickets = new ObservableCollection<Ticket>(filtered);
            OnPropertyChanged(nameof(FilteredTickets));
        }

        private void UpdateFilteredUsers()
        {
            var filtered = Users
                .Where(u => (string.IsNullOrEmpty(UserSearchQuery) ||
                            u.Email.Contains(UserSearchQuery, StringComparison.OrdinalIgnoreCase) ||
                            u.LastName.Contains(UserSearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                            (!ShowAdminsOnly || u.IsAdmin))
                .ToList();

            FilteredUsers = new ObservableCollection<User>(filtered);
            OnPropertyChanged(nameof(FilteredUsers));
        }

        private void DeleteTicket()
        {
            if (SelectedTicket == null) return;

            if (MessageBox.Show("Удалить этот маршрут?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM Tickets WHERE Id = @Id", conn))
                {
                    cmd.Parameters.AddWithValue("@Id", SelectedTicket.Id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Tickets.Remove(SelectedTicket);
                    UpdateFilteredTickets();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при удалении: {ex.Message}");
            }
        }

        private void SaveTicket()
        {
            if (SelectedTicket == null) return;

            SelectedTicket.Type = MapTransportType(SelectedTicket.Type);

            if (!ValidateTicket(SelectedTicket, out var errorMessage))
            {
                MessageBox.Show(errorMessage, "Ошибки ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (SelectedTicket.Id == 0) // Новый билет
                    {
                        var cmd = new SqlCommand(
                            @"INSERT INTO Tickets ([From], [To], Date, Time, Price, Number, Description, Type)
                    VALUES (@From, @To, @Date, @Time, @Price, @Number, @Description, @Type);
                    SELECT SCOPE_IDENTITY();", conn);

                        SetTicketParameters(cmd);
                        // Получаем новый Id из базы
                        SelectedTicket.Id = Convert.ToInt32(cmd.ExecuteScalar());
                        Tickets.Add(SelectedTicket);
                    }
                    else
                    {
                        var cmd = new SqlCommand(
                            @"UPDATE Tickets SET 
                                [From] = @From, 
                                [To] = @To,
                                Date = @Date,
                                Time = @Time,
                                Price = @Price,
                                Number = @Number,
                                Description = @Description,
                                Type = @Type
                            WHERE Id = @Id", conn);

                        cmd.Parameters.AddWithValue("@Id", SelectedTicket.Id);
                        SetTicketParameters(cmd);
                        cmd.ExecuteNonQuery();
                    }

                    UpdateFilteredTickets();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
            MessageBox.Show("Маршрут успешно сохранён!", "Успех",
            MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void SetTicketParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@From", SelectedTicket.From);
            cmd.Parameters.AddWithValue("@To", SelectedTicket.To);
            cmd.Parameters.AddWithValue("@Date", SelectedTicket.Date);
            cmd.Parameters.AddWithValue("@Time", SelectedTicket.Time);
            cmd.Parameters.AddWithValue("@Price", SelectedTicket.Price);
            cmd.Parameters.AddWithValue("@Number", SelectedTicket.Number);
            cmd.Parameters.AddWithValue("@Description", SelectedTicket.Description ?? "");
            cmd.Parameters.AddWithValue("@Type", SelectedTicket.Type);
        }

        private void AddTicket()
        {
            SelectedTicket = new Ticket
            {
                Date = DateTime.Today,
                Time = DateTime.Now.ToString("HH:mm"),
                Price = 0,
                Number = 0,
                Type = TransportTypesForEdit.FirstOrDefault()
            };
        }

        private void DeleteUser()
        {
            if (SelectedUser == null) return;

            if (MessageBox.Show("Удалить этого пользователя?", "Подтверждение",
                MessageBoxButton.YesNo) == MessageBoxResult.No) return;

            try
            {
                using (var conn = new SqlConnection(connectionString))
                using (var cmd = new SqlCommand("DELETE FROM Users WHERE Email = @Email", conn))
                {
                    cmd.Parameters.AddWithValue("@Email", SelectedUser.Email);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Users.Remove(SelectedUser);
                    UpdateFilteredUsers();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }

        private void SaveUser()
        {
            if (SelectedUser == null) return;

            if (!ValidateUser(SelectedUser, out var errorMessage))
            {
                MessageBox.Show(errorMessage, "Ошибки ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    if (!Users.Any(u => u.Email == SelectedUser.Email))
                    {
                        var cmd = new SqlCommand(
                            @"INSERT INTO Users 
                    (FirstName, LastName, Surname, Email, PasswordHash, PhoneNumber, isAdmin)
                    VALUES (@FirstName, @LastName, @Surname, @Email, @PasswordHash, @PhoneNumber, @IsAdmin)",
                            conn);

                        SetUserParameters(cmd);
                        cmd.ExecuteNonQuery();
                        Users.Add(SelectedUser);
                    }
                    else
                    {
                        var cmd = new SqlCommand(
                            @"UPDATE Users SET 
                        FirstName = @FirstName,
                        LastName = @LastName,
                        Surname = @Surname,
                        PhoneNumber = @PhoneNumber,
                        isAdmin = @IsAdmin,
                        PasswordHash = @PasswordHash
                    WHERE Email = @Email",
                            conn);

                        SetUserParameters(cmd);
                        cmd.ExecuteNonQuery();
                    }
                    UpdateFilteredUsers();
                }
                MessageBox.Show("Пользователь успешно сохранён!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void SetUserParameters(SqlCommand cmd)
        {
            cmd.Parameters.AddWithValue("@FirstName", SelectedUser.FirstName ?? "");
            cmd.Parameters.AddWithValue("@LastName", SelectedUser.LastName ?? "");
            cmd.Parameters.AddWithValue("@Surname", SelectedUser.Surname ?? "");
            cmd.Parameters.AddWithValue("@Email", SelectedUser.Email);
            cmd.Parameters.AddWithValue("@PasswordHash", SelectedUser.PasswordHash);
            cmd.Parameters.AddWithValue("@PhoneNumber", SelectedUser.PhoneNumber ?? "");
            cmd.Parameters.AddWithValue("@IsAdmin", SelectedUser.IsAdmin);
        }

        private void AddUser()
        {
            SelectedUser = new User
            {
                FirstName = "",
                LastName = "",
                Surname = "",
                Email = "",
                PasswordHash = HashPassword(""),
                PhoneNumber = "",
                IsAdmin = false
            };
        }
        private ObservableCollection<Ticket> LoadTickets()
        {
            var tickets = new ObservableCollection<Ticket>();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM Tickets", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var dbType = reader["Type"].ToString();
                        var localizedType = _transportTypeMap.FirstOrDefault(x => x.Value == dbType).Key ?? dbType;
                        var displayType = Application.Current.Resources[localizedType]?.ToString() ?? dbType;

                        tickets.Add(new Ticket
                        {
                            Id = reader.GetInt32(0),
                            From = reader.GetString(1),
                            To = reader.GetString(2),
                            Date = reader.GetDateTime(3),
                            Time = reader.GetTimeSpan(4).ToString(@"hh\:mm"),
                            Price = reader.GetDouble(5),
                            Number = reader.GetInt32(6),
                            Description = reader.GetString(7),
                            Type = displayType // Используем локализованное значение
                        });
                    }
                }
            }
            return tickets;
        }

        private ObservableCollection<User> LoadUsers()
        {
            var users = new ObservableCollection<User>();
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM Users", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            FirstName = reader.GetString(0),
                            LastName = reader.GetString(1),
                            Surname = reader.GetString(2),
                            Email = reader.GetString(3),
                            PasswordHash = reader.GetString(4),
                            PhoneNumber = reader.IsDBNull(5) ? null : reader.GetString(5),
                            IsAdmin = reader.GetBoolean(6)
                        });
                    }
                }
            }
            return users;
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}