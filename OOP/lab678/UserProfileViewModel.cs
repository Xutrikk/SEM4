using System;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace RouteBookingSystem
{
    public class UserProfileViewModel : INotifyPropertyChanged
    {
        private readonly string _connectionString = "Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;";
        private readonly string _email;
        private string _firstName;
        private string _lastName;
        private string _surname;
        private string _phoneNumber;
        private readonly PasswordBox _passwordBox;
        private readonly PasswordBox _confirmPasswordBox;

        public UserProfileViewModel(string email, PasswordBox passwordBox, PasswordBox confirmPasswordBox)
        {
            _email = email;
            _passwordBox = passwordBox;
            _confirmPasswordBox = confirmPasswordBox;
            LoadUserData();
            SaveProfileCommand = new RelayCommand(_ => SaveProfile());
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
            }
        }

        public string Surname
        {
            get => _surname;
            set
            {
                _surname = value;
                OnPropertyChanged();
            }
        }

        public string Email
        {
            get => _email;
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand SaveProfileCommand { get; }

        private void LoadUserData()
        {
            try
            {
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand("SELECT FirstName, LastName, Surname, PhoneNumber FROM Users WHERE Email = @Email", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@Email", _email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            FirstName = reader.GetString(0);
                            LastName = reader.GetString(1);
                            Surname = reader.GetString(2);
                            PhoneNumber = reader.IsDBNull(3) ? null : reader.GetString(3);
                        }
                        else
                        {
                            MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveProfile()
        {
            try
            {
                // Валидация полей
                if (!ValidateInputs()) return;

                // Проверка пароля
                string newPassword = _passwordBox.Password;
                string confirmPassword = _confirmPasswordBox.Password;
                string hashedPassword = null;

                if (!string.IsNullOrEmpty(newPassword) || !string.IsNullOrEmpty(confirmPassword))
                {
                    if (newPassword != confirmPassword)
                    {
                        MessageBox.Show("Пароли не совпадают!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    if (string.IsNullOrEmpty(newPassword))
                    {
                        MessageBox.Show("Введите новый пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                    hashedPassword = HashPassword(newPassword);
                }

                // Сохранение изменений в базе данных
                using (var conn = new SqlConnection(_connectionString))
                using (var cmd = new SqlCommand(
                    "UPDATE Users SET FirstName = @FirstName, LastName = @LastName, Surname = @Surname, PhoneNumber = @PhoneNumber" +
                    (hashedPassword != null ? ", PasswordHash = @PasswordHash" : "") +
                    " WHERE Email = @Email", conn))
                {
                    conn.Open();
                    cmd.Parameters.AddWithValue("@FirstName", FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Surname", Surname ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PhoneNumber", (object)PhoneNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", _email);
                    if (hashedPassword != null)
                    {
                        cmd.Parameters.AddWithValue("@PasswordHash", hashedPassword);
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Профиль успешно обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка при обновлении профиля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Очистка полей пароля после сохранения
                _passwordBox.Clear();
                _confirmPasswordBox.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            var errors = new StringBuilder();
            bool isValid = true;

            // Обязательные поля
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                errors.AppendLine("• Поле 'Имя' обязательно");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                errors.AppendLine("• Поле 'Фамилия' обязательно");
                isValid = false;
            }
            if (string.IsNullOrWhiteSpace(Surname))
            {
                errors.AppendLine("• Поле 'Отчество' обязательно");
                isValid = false;
            }

            // Формат телефона (+375XXXXXXXXX)
            if (!string.IsNullOrEmpty(PhoneNumber) && !Regex.IsMatch(PhoneNumber, @"^\+375(25|29|33|44)\d{7}$"))
            {
                errors.AppendLine("• Телефон должен быть в формате: +375XXXXXXXXX");
                isValid = false;
            }

            // Пароль
            if (!string.IsNullOrEmpty(_passwordBox.Password) && _passwordBox.Password.Length < 6)
            {
                errors.AppendLine("• Пароль должен содержать минимум 6 символов");
                isValid = false;
            }

            if (!isValid)
            {
                MessageBox.Show(errors.ToString(), "Ошибки ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            return isValid;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}