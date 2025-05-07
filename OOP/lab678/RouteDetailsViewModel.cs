using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using lab4wpf5oop.Models;

namespace RouteBookingSystem
{
    public class RouteDetailsViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString = "Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;";
        private readonly string _email;
        private readonly int _numberOfSeats;
        private string _selectedBoardingPoint;
        private string _selectedDropOffPoint;
        private string _purchaseStatus;
        private ObservableCollection<string> _boardingPoints;
        private ObservableCollection<string> _dropOffPoints;

        public Ticket Ticket { get; }
        public RelayCommand AddToFavoritesCommand { get; }
        public RelayCommand BookTicketCommand { get; }
        public RelayCommand PayTicketCommand { get; }
        public int NumberOfSeats => _numberOfSeats; // Количество мест
        public decimal CalculatedPrice => (decimal)(Ticket.Price * _numberOfSeats); // Цена с учётом количества мест

        public string SelectedBoardingPoint
        {
            get => _selectedBoardingPoint;
            set
            {
                _selectedBoardingPoint = value;
                OnPropertyChanged();
            }
        }

        public string SelectedDropOffPoint
        {
            get => _selectedDropOffPoint;
            set
            {
                _selectedDropOffPoint = value;
                OnPropertyChanged();
            }
        }

        public string PurchaseStatus
        {
            get => _purchaseStatus;
            set
            {
                _purchaseStatus = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> BoardingPoints
        {
            get => _boardingPoints;
            set
            {
                _boardingPoints = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> DropOffPoints
        {
            get => _dropOffPoints;
            set
            {
                _dropOffPoints = value;
                OnPropertyChanged();
            }
        }

        public RouteDetailsViewModel(string email, Ticket ticket, int numberOfSeats)
        {
            _email = email;
            _numberOfSeats = numberOfSeats;
            Ticket = ticket;
            AddToFavoritesCommand = new RelayCommand(_ => AddToFavorites());
            BookTicketCommand = new RelayCommand(_ => BookTicket());
            PayTicketCommand = new RelayCommand(_ => PayTicket());
            LoadBoardingAndDropOffPoints();
            CheckPurchaseStatus();
        }

        private void LoadBoardingAndDropOffPoints()
        {
            var points = new[]
            {
                ("Минск", "Центральный вокзал", 1, 1),
                ("Минск", "Площадь Победы", 1, 1),
                ("Минск", "Автовокзал Восточный", 1, 0),
                ("Минск", "Станция метро Каменная Горка", 0, 1),
                ("Гродно", "Автовокзал Гродно", 1, 1),
                ("Гродно", "Железнодорожный вокзал", 1, 0),
                ("Гродно", "ТЦ OldCity", 0, 1),
                ("Брест", "Брестский вокзал", 1, 1),
                ("Брест", "Автостанция Брест", 1, 0),
                ("Брест", "ТЦ Корона", 0, 1),
                ("Витебск", "Витебский вокзал", 1, 1),
                ("Витебск", "Площадь Свободы", 1, 0),
                ("Витебск", "ТЦ Марко-Сити", 0, 1),
                ("Могилев", "Могилевский вокзал", 1, 1),
                ("Могилев", "Автостанция Могилев", 1, 0),
                ("Могилев", "Парк Горького", 0, 1),
                ("Гомель", "Гомельский вокзал", 1, 1),
                ("Гомель", "Автовокзал Гомель", 1, 0),
                ("Гомель", "ТЦ Секрет", 0, 1),
                ("Пинск", "Пинский вокзал", 1, 1),
                ("Пинск", "Автостанция Пинск", 1, 0),
                ("Пинск", "Центральная площадь", 0, 1),
                ("Лида", "Лидский вокзал", 1, 1),
                ("Лида", "Автостанция Лида", 1, 0),
                ("Лида", "ТЦ Лида", 0, 1)
            };

            BoardingPoints = new ObservableCollection<string>(
                points.Where(p => p.Item1 == Ticket.From && p.Item3 == 1).Select(p => p.Item2));
            DropOffPoints = new ObservableCollection<string>(
                points.Where(p => p.Item1 == Ticket.To && p.Item4 == 1).Select(p => p.Item2));

            SelectedBoardingPoint = BoardingPoints.FirstOrDefault();
            SelectedDropOffPoint = DropOffPoints.FirstOrDefault();
        }

        private void CheckPurchaseStatus()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    var cmd = new SqlCommand(
                        "SELECT Status FROM PurchasedTickets WHERE EmailUser = @Email AND TicketId = @TicketId",
                        conn);
                    cmd.Parameters.AddWithValue("@Email", _email);
                    cmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    var result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        PurchaseStatus = (int)result == 0 ? "Не оплачено" : "Оплачено";
                    }
                    else
                    {
                        PurchaseStatus = "Не забронировано";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка проверки статуса: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                PurchaseStatus = "Неизвестно";
            }
        }

        private void AddToFavorites()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();
                    // Проверка существования маршрута в избранном
                    var checkCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM Favorites WHERE EmailUser = @Email AND TicketId = @TicketId",
                        conn);
                    checkCmd.Parameters.AddWithValue("@Email", _email);
                    checkCmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    int count = (int)checkCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        MessageBox.Show("Этот маршрут уже добавлен в избранное!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    var cmd = new SqlCommand(
                        @"INSERT INTO Favorites (EmailUser, TicketId, AddedDate) 
                          VALUES (@EmailUser, @TicketId, @AddedDate)",
                        conn);
                    cmd.Parameters.AddWithValue("@EmailUser", _email);
                    cmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    cmd.Parameters.AddWithValue("@AddedDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Маршрут добавлен в избранное!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BookTicket()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Проверяем, не забронирован ли маршрут уже
                    var checkDuplicateCmd = new SqlCommand(
                        "SELECT COUNT(*) FROM PurchasedTickets WHERE EmailUser = @Email AND TicketId = @TicketId",
                        conn);
                    checkDuplicateCmd.Parameters.AddWithValue("@Email", _email);
                    checkDuplicateCmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    int duplicateCount = (int)checkDuplicateCmd.ExecuteScalar();

                    if (duplicateCount > 0)
                    {
                        MessageBox.Show("Этот маршрут уже забронирован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Проверяем, достаточно ли билетов
                    var checkCmd = new SqlCommand(
                        "SELECT Number FROM Tickets WHERE Id = @TicketId", conn);
                    checkCmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    int availableTickets = (int)checkCmd.ExecuteScalar();

                    if (availableTickets < _numberOfSeats)
                    {
                        MessageBox.Show("Недостаточно доступных мест!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Уменьшаем количество доступных мест
                    var updateCmd = new SqlCommand(
                        "UPDATE Tickets SET Number = Number - @NumberOfSeats WHERE Id = @TicketId",
                        conn);
                    updateCmd.Parameters.AddWithValue("@NumberOfSeats", _numberOfSeats);
                    updateCmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    updateCmd.ExecuteNonQuery();

                    // Сохраняем бронирование
                    var insertCmd = new SqlCommand(
                        @"INSERT INTO PurchasedTickets 
                          (PurchaseId, EmailUser, TicketId, [From], [To], PurchaseDate, PurchaseTime, Price, Number, Status, [Type], BoardingPoints, DropOffPoints) 
                          VALUES 
                          ((SELECT ISNULL(MAX(PurchaseId), 0) + 1 FROM PurchasedTickets), @EmailUser, @TicketId, @From, @To, @PurchaseDate, @PurchaseTime, @Price, @Number, @Status, @Type, @BoardingPoints, @DropOffPoints)",
                        conn);
                    insertCmd.Parameters.AddWithValue("@EmailUser", _email);
                    insertCmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    insertCmd.Parameters.AddWithValue("@From", Ticket.From);
                    insertCmd.Parameters.AddWithValue("@To", Ticket.To);
                    insertCmd.Parameters.AddWithValue("@PurchaseDate", DateTime.Now.Date);
                    insertCmd.Parameters.AddWithValue("@PurchaseTime", DateTime.Now.ToString("HH:mm"));
                    insertCmd.Parameters.AddWithValue("@Price", (decimal)(Ticket.Price * _numberOfSeats));
                    insertCmd.Parameters.AddWithValue("@Number", _numberOfSeats);
                    insertCmd.Parameters.AddWithValue("@Status", 0); // Статус: не оплачено (0)
                    insertCmd.Parameters.AddWithValue("@Type", Ticket.Type);
                    insertCmd.Parameters.AddWithValue("@BoardingPoints", SelectedBoardingPoint);
                    insertCmd.Parameters.AddWithValue("@DropOffPoints", SelectedDropOffPoint);
                    insertCmd.ExecuteNonQuery();
                }
                MessageBox.Show($"Маршрут успешно забронирован! Количество мест: {_numberOfSeats}", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                CheckPurchaseStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка бронирования: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void PayTicket()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Проверяем, забронирован ли маршрут
                    var checkCmd = new SqlCommand(
                        "SELECT Status FROM PurchasedTickets WHERE EmailUser = @Email AND TicketId = @TicketId",
                        conn);
                    checkCmd.Parameters.AddWithValue("@Email", _email);
                    checkCmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                    var result = checkCmd.ExecuteScalar();

                    if (result == null)
                    {
                        MessageBox.Show("Сначала забронируйте маршрут!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if ((int)result == 1)
                    {
                        MessageBox.Show("Маршрут уже оплачен!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Открываем окно оплаты
                    var paymentWindow = new PaymentWindow((decimal)(Ticket.Price * _numberOfSeats));
                    if (paymentWindow.ShowDialog() == true)
                    {
                        // Обновляем статус на "Оплачено"
                        var updateCmd = new SqlCommand(
                            "UPDATE PurchasedTickets SET Status = 1 WHERE EmailUser = @Email AND TicketId = @TicketId",
                            conn);
                        updateCmd.Parameters.AddWithValue("@Email", _email);
                        updateCmd.Parameters.AddWithValue("@TicketId", Ticket.Id);
                        updateCmd.ExecuteNonQuery();

                        MessageBox.Show("Оплата успешно выполнена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        CheckPurchaseStatus();

                        var dashboardWindow = new UserDashboardWindow(_email);
                        dashboardWindow.Show();
                        (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is RouteDetailsWindow))?.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка оплаты: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}