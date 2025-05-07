using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Collections.Generic;
using lab4wpf5oop.Models;

namespace RouteBookingSystem
{
    public class SearchRoutesViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString = "Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;";
        private readonly string _email;
        private string _from;
        private string _to;
        private DateTime _date = DateTime.Today;
        private int _numberOfSeats = 1; // Установлено начальное значение 1
        private string _selectedTransportType;
        private ObservableCollection<string> _transportTypes;
        private ObservableCollection<Ticket> _filteredTickets;
        private Ticket _selectedTicket;
        private ObservableCollection<string> _fromOptions;
        private ObservableCollection<string> _toOptions;
        private ObservableCollection<int> _seatOptions; // Для ComboBox количества билетов

        public SearchRoutesViewModel(string email)
        {
            _email = email;
            LoadTransportTypes();
            LoadRouteOptions();
            LoadSeatOptions();
            FilteredTickets = new ObservableCollection<Ticket>();
            SearchRoutesCommand = new RelayCommand(_ => SearchRoutes());
            SelectRouteCommand = new RelayCommand(_ => SelectRoute());
        }

        public string From
        {
            get => _from;
            set
            {
                if (_from != value)
                {
                    _from = value;
                    OnPropertyChanged();
                    UpdateToOptions();
                }
            }
        }

        public string To
        {
            get => _to;
            set
            {
                if (_to != value)
                {
                    _to = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged();
            }
        }

        public int NumberOfSeats
        {
            get => _numberOfSeats;
            set
            {
                if (value < 1) value = 1;
                if (value > 3) value = 3;
                _numberOfSeats = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<int> SeatOptions
        {
            get => _seatOptions;
            set
            {
                _seatOptions = value;
                OnPropertyChanged();
            }
        }

        public string SelectedTransportType
        {
            get => _selectedTransportType;
            set
            {
                _selectedTransportType = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> TransportTypes
        {
            get => _transportTypes;
            set
            {
                _transportTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Ticket> FilteredTickets
        {
            get => _filteredTickets;
            set
            {
                _filteredTickets = value;
                OnPropertyChanged();
            }
        }

        public Ticket SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                _selectedTicket = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> FromOptions
        {
            get => _fromOptions;
            set
            {
                _fromOptions = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ToOptions
        {
            get => _toOptions;
            set
            {
                _toOptions = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SearchRoutesCommand { get; }
        public RelayCommand SelectRouteCommand { get; }

        private void LoadTransportTypes()
        {
            TransportTypes = new ObservableCollection<string>
            {
                Application.Current.Resources["AllTypes"]?.ToString() ?? "All Types",
                Application.Current.Resources["Bus"]?.ToString() ?? "Bus",
                Application.Current.Resources["Marshrutka"]?.ToString() ?? "Marshrutka"
            };
            SelectedTransportType = TransportTypes[0];
        }

        private void LoadSeatOptions()
        {
            SeatOptions = new ObservableCollection<int> { 1, 2, 3 };
            NumberOfSeats = 1; // Устанавливаем начальное значение
        }

        private void LoadRouteOptions()
        {
            var routes = new List<(string From, string To)>
            {
                ("Минск", "Гродно"), ("Минск", "Брест"), ("Минск", "Витебск"),
                ("Минск", "Могилев"), ("Минск", "Гомель"), ("Гродно", "Брест"),
                ("Брест", "Гомель"), ("Витебск", "Могилев"), ("Могилев", "Гомель"),
                ("Брест", "Витебск"), ("Пинск", "Минск"), ("Лида", "Минск"),
                ("Гомель", "Минск"), ("Витебск", "Брест"), ("Гродно", "Могилев"),
                ("Гродно", "Витебск"), ("Брест", "Лида"), ("Могилев", "Лида"),
                ("Пинск", "Гомель"), ("Лида", "Витебск")
            };

            var fromSet = routes.Select(r => r.From).Distinct().OrderBy(x => x).ToList();
            FromOptions = new ObservableCollection<string>(fromSet);
            From = FromOptions.FirstOrDefault();
            UpdateToOptions();
        }

        private void UpdateToOptions()
        {
            if (string.IsNullOrEmpty(From))
            {
                ToOptions = new ObservableCollection<string>();
                To = null;
                return;
            }

            var routes = new List<(string From, string To)>
            {
                ("Минск", "Гродно"), ("Минск", "Брест"), ("Минск", "Витебск"),
                ("Минск", "Могилев"), ("Минск", "Гомель"), ("Гродно", "Брест"),
                ("Брест", "Гомель"), ("Витебск", "Могилев"), ("Могилев", "Гомель"),
                ("Брест", "Витебск"), ("Пинск", "Минск"), ("Лида", "Минск"),
                ("Гомель", "Минск"), ("Витебск", "Брест"), ("Гродно", "Могилев"),
                ("Гродно", "Витебск"), ("Брест", "Лида"), ("Могилев", "Лида"),
                ("Пинск", "Гомель"), ("Лида", "Витебск")
            };

            var toSet = routes.Where(r => r.From == From)
                             .Select(r => r.To)
                             .Distinct()
                             .OrderBy(x => x)
                             .ToList();

            ToOptions = new ObservableCollection<string>(toSet);

            if (!string.IsNullOrEmpty(To) && !ToOptions.Contains(To))
            {
                To = null;
            }

            if (ToOptions.Any() && string.IsNullOrEmpty(To))
            {
                To = ToOptions.First();
            }
        }

        private void SearchRoutes()
        {
            try
            {
                if (string.IsNullOrEmpty(From) || string.IsNullOrEmpty(To))
                {
                    MessageBox.Show("Пожалуйста, выберите 'Откуда' и 'Куда'!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var tickets = new ObservableCollection<Ticket>();
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("SELECT * FROM Tickets", conn))
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
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
                                Type = reader.GetString(8),
                                BoardingPoints = reader.GetString(9),
                                DropOffPoints = reader.GetString(10)
                            });
                        }
                    }
                }

                var filtered = tickets
                    .Where(t => t.From == From && t.To == To)
                    .Where(t => t.Date.Date == Date.Date)
                    // Удаляем фильтрацию по количеству билетов, чтобы показывать все маршруты
                    .Where(t => SelectedTransportType == TransportTypes[0] || t.Type == SelectedTransportType)
                    .ToList();

                FilteredTickets = new ObservableCollection<Ticket>(filtered);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка поиска: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SelectRoute()
        {
            if (SelectedTicket == null)
            {
                MessageBox.Show("Пожалуйста, выберите маршрут!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Проверяем, достаточно ли билетов перед переходом
            if (SelectedTicket.Number < NumberOfSeats)
            {
                MessageBox.Show("Недостаточно доступных мест!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var routeDetailsWindow = new RouteDetailsWindow(_email, SelectedTicket, NumberOfSeats);
            routeDetailsWindow.Show();
            (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is SearchRoutesWindow))?.Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}