using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Windows;
using lab4wpf5oop.Models;

namespace RouteBookingSystem
{
    public class PurchaseHistoryViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString = "Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;";
        private readonly string _email;
        private ObservableCollection<PurchasedTicket> _purchasedTickets;
        private PurchasedTicket _selectedTicket;

        public PurchaseHistoryViewModel(string email)
        {
            _email = email;
            PurchasedTickets = new ObservableCollection<PurchasedTicket>();
            LoadPurchasedTickets();
        }

        public ObservableCollection<PurchasedTicket> PurchasedTickets
        {
            get => _purchasedTickets;
            set
            {
                _purchasedTickets = value;
                OnPropertyChanged();
            }
        }

        public PurchasedTicket SelectedTicket
        {
            get => _selectedTicket;
            set
            {
                _selectedTicket = value;
                OnPropertyChanged();
            }
        }

        private void LoadPurchasedTickets()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(
                    "SELECT pt.PurchaseId, pt.EmailUser, pt.TicketId, pt.[From], pt.[To], t.[Date], t.[Time], pt.Price, pt.Number, pt.Status, pt.[Type], pt.BoardingPoints, pt.DropOffPoints " +
                    "FROM PurchasedTickets pt JOIN Tickets t ON pt.TicketId = t.Id WHERE pt.EmailUser = @Email", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Email", _email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            PurchasedTickets.Add(new PurchasedTicket
                            {
                                PurchaseId = reader.GetInt32(0),
                                EmailUser = reader.GetString(1),
                                TicketId = reader.GetInt32(2),
                                From = reader.GetString(3),
                                To = reader.GetString(4),
                                Date = reader.GetDateTime(5),
                                Time = reader.GetTimeSpan(6).ToString(@"hh\:mm"),
                                Price = reader.GetDouble(7),
                                Number = reader.GetInt32(8),
                                Status = reader.GetInt32(9),
                                Type = reader.GetString(10),
                                BoardingPoints = reader.GetString(11),
                                DropOffPoints = reader.GetString(12)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки истории покупок: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void ShowRatingDialog(int purchaseId)
        {
            var ticket = PurchasedTickets.FirstOrDefault(t => t.PurchaseId == purchaseId);
            if (ticket == null) return;

            var ratingWindow = new RatingWindow(_email, purchaseId);
            ratingWindow.ShowDialog();
        }

        public void CancelTicket(int purchaseId)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Находим TicketId и Number (количество заказанных билетов)
                    int ticketId = 0;
                    int numberOfSeats = 0;
                    using (var cmd = new SqlCommand(
                        "SELECT TicketId, Number FROM PurchasedTickets WHERE PurchaseId = @PurchaseId AND EmailUser = @Email", conn))
                    {
                        cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
                        cmd.Parameters.AddWithValue("@Email", _email);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                ticketId = reader.GetInt32(0);
                                numberOfSeats = reader.GetInt32(1);
                            }
                            else
                            {
                                MessageBox.Show("Покупка не найдена!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }

                    // Возвращаем количество билетов в таблицу Tickets
                    using (var cmd = new SqlCommand(
                        "UPDATE Tickets SET Number = Number + @Number WHERE Id = @TicketId", conn))
                    {
                        cmd.Parameters.AddWithValue("@Number", numberOfSeats);
                        cmd.Parameters.AddWithValue("@TicketId", ticketId);
                        cmd.ExecuteNonQuery();
                    }

                    // Удаляем запись из PurchasedTickets
                    using (var cmd = new SqlCommand(
                        "DELETE FROM PurchasedTickets WHERE PurchaseId = @PurchaseId AND EmailUser = @Email", conn))
                    {
                        cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
                        cmd.Parameters.AddWithValue("@Email", _email);
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            PurchasedTickets.Remove(PurchasedTickets.FirstOrDefault(t => t.PurchaseId == purchaseId));
                            MessageBox.Show("Поездка отменена, билеты возвращены!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка отмены поездки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveRating(int purchaseId, int rating, string comment)
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(
                    "INSERT INTO TripRatings (PurchaseId, EmailUser, Rating, Comment, RatingDate) VALUES (@PurchaseId, @Email, @Rating, @Comment, @RatingDate)",
                    conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
                    cmd.Parameters.AddWithValue("@Email", _email);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Comment", string.IsNullOrEmpty(comment) ? (object)DBNull.Value : comment);
                    cmd.Parameters.AddWithValue("@RatingDate", DateTime.Now);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Оценка успешно сохранена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения оценки: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public class PurchasedTicket : INotifyPropertyChanged
        {
            private int _purchaseId;
            private string _emailUser;
            private int _ticketId;
            private string _from;
            private string _to;
            private DateTime _date;
            private string _time;
            private double _price;
            private int _number;
            private int _status;
            private string _type;
            private string _boardingPoints;
            private string _dropOffPoints;

            public int PurchaseId
            {
                get => _purchaseId;
                set { _purchaseId = value; OnPropertyChanged(); }
            }

            public string EmailUser
            {
                get => _emailUser;
                set { _emailUser = value; OnPropertyChanged(); }
            }

            public int TicketId
            {
                get => _ticketId;
                set { _ticketId = value; OnPropertyChanged(); }
            }

            public string From
            {
                get => _from;
                set { _from = value; OnPropertyChanged(); }
            }

            public string To
            {
                get => _to;
                set { _to = value; OnPropertyChanged(); }
            }

            public DateTime Date
            {
                get => _date;
                set { _date = value; OnPropertyChanged(); }
            }

            public string Time
            {
                get => _time;
                set { _time = value; OnPropertyChanged(); }
            }

            public double Price
            {
                get => _price;
                set { _price = value; OnPropertyChanged(); }
            }

            public int Number
            {
                get => _number;
                set { _number = value; OnPropertyChanged(); }
            }

            public int Status
            {
                get => _status;
                set
                {
                    _status = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(StatusText));
                }
            }

            public string StatusText
            {
                get => Status == 0 ? "Не оплачено" : "Оплачено";
            }

            public string Type
            {
                get => _type;
                set { _type = value; OnPropertyChanged(); }
            }

            public string BoardingPoints
            {
                get => _boardingPoints;
                set { _boardingPoints = value; OnPropertyChanged(); }
            }

            public string DropOffPoints
            {
                get => _dropOffPoints;
                set { _dropOffPoints = value; OnPropertyChanged(); }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}