using lab4wpf5oop.Models;
using Microsoft.EntityFrameworkCore;
using RouteBookingSystem.Data;
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace RouteBookingSystem
{
    public partial class RegistrationWindow : Window, INotifyPropertyChanged
    {
        private string _firstName, _lastName, _surname, _email, _phone;
        private readonly IUnitOfWork _unitOfWork;

        public string FirstName
        {
            get => _firstName;
            set { _firstName = value; OnPropertyChanged(); }
        }

        public string LastName
        {
            get => _lastName;
            set { _lastName = value; OnPropertyChanged(); }
        }

        public string Surname
        {
            get => _surname;
            set { _surname = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        public RegistrationWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("C:/BSTU/SEM4/OOP/lab4wpf5oop/lab4wpf5oop/Images/bus_icon-icons.com_76529.ico"));
            DataContext = this;
            InitializePlaceholders();
            var options = new DbContextOptionsBuilder<XKKBUSContext>()
                .UseSqlServer("Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;
            _unitOfWork = new UnitOfWork(new XKKBUSContext(options));
        }

        private void InitializePlaceholders()
        {
            txtFirstName.TextChanged += (s, e) => UpdatePlaceholderVisibility(txtFirstName, firstNamePlaceholder);
            txtLastName.TextChanged += (s, e) => UpdatePlaceholderVisibility(txtLastName, lastNamePlaceholder);
            txtSurname.TextChanged += (s, e) => UpdatePlaceholderVisibility(txtSurname, surnamePlaceholder);
            txtEmail.TextChanged += (s, e) => UpdatePlaceholderVisibility(txtEmail, emailPlaceholder);
            txtPhone.TextChanged += (s, e) => UpdatePlaceholderVisibility(txtPhone, phonePlaceholder);
        }

        private void UpdatePlaceholderVisibility(TextBox textBox, TextBlock placeholder)
        {
            placeholder.Visibility = string.IsNullOrEmpty(textBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            ResetFieldBorders();
            if (!ValidateInputs()) return;

            try
            {
                if (await IsEmailExists(Email))
                {
                    MessageBox.Show("Email уже зарегистрирован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    HighlightErrorField(txtEmail);
                    return;
                }

                var newUser = new User
                {
                    FirstName = FirstName,
                    LastName = LastName,
                    Surname = Surname,
                    Email = Email,
                    PasswordHash = HashPassword(txtPassword.Password),
                    PhoneNumber = Phone,
                    IsAdmin = false,
                    IsBlocked = false
                };

                await _unitOfWork.Users.AddAsync(newUser);
                await _unitOfWork.SaveChangesAsync();

                MessageBox.Show("Регистрация успешна!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                new LoginWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ValidateInputs()
        {
            var errors = new StringBuilder();
            bool isValid = true;

            CheckField(FirstName, "Имя", txtFirstName, errors);
            CheckField(LastName, "Фамилия", txtLastName, errors);
            CheckField(Surname, "Отчество", txtSurname, errors);
            CheckField(Email, "Email", txtEmail, errors);
            CheckField(Phone, "Телефон", txtPhone, errors);

            if (string.IsNullOrEmpty(txtPassword.Password))
            {
                errors.AppendLine("• Поле 'Пароль' обязательно");
                HighlightErrorField(txtPassword);
                isValid = false;
            }

            if (!string.IsNullOrEmpty(FirstName) && !Regex.IsMatch(FirstName, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("• Имя должно содержать только русские или английские буквы");
                HighlightErrorField(txtFirstName);
                isValid = false;
            }

            if (!string.IsNullOrEmpty(LastName) && !Regex.IsMatch(LastName, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("• Фамилия должна содержать только русские или английские буквы");
                HighlightErrorField(txtLastName);
                isValid = false;
            }

            if (!string.IsNullOrEmpty(Surname) && !Regex.IsMatch(Surname, @"^[а-яА-ЯёЁa-zA-Z]+$"))
            {
                errors.AppendLine("• Отчество должно содержать только русские или английские буквы");
                HighlightErrorField(txtSurname);
                isValid = false;
            }

            if (!string.IsNullOrEmpty(Email) && !Regex.IsMatch(Email, @"^[a-zA-Z][a-zA-Z0-9]*@[a-zA-Z][a-zA-Z0-9]*\.[a-zA-Z]+$"))
            {
                errors.AppendLine("• Неверный формат Email (только английские буквы и цифры, без спецсимволов (кроме @ и .), без пробелов)");
                HighlightErrorField(txtEmail);
                isValid = false;
            }

            if (!string.IsNullOrEmpty(Phone) && !Regex.IsMatch(Phone, @"^\+375(25|29|33|44)\d{7}$"))
            {
                errors.AppendLine("• Телефон должен быть в формате: +375XXXXXXXXX");
                HighlightErrorField(txtPhone);
                isValid = false;
            }

            if (txtPassword.Password != txtConfirmPassword.Password)
            {
                errors.AppendLine("• Пароли не совпадают");
                HighlightErrorField(txtPassword);
                HighlightErrorField(txtConfirmPassword);
                isValid = false;
            }

            if (!string.IsNullOrEmpty(txtPassword.Password))
            {
                if (txtPassword.Password.Length < 6)
                {
                    errors.AppendLine("• Пароль должен содержать минимум 6 символов");
                    HighlightErrorField(txtPassword);
                    isValid = false;
                }

                if (!Regex.IsMatch(txtPassword.Password, @"^[a-zA-Z0-9!@#$%^&*()+\-=\[\]{};:'"",.<>?]+$") || txtPassword.Password.Contains(" "))
                {
                    errors.AppendLine("• Пароль должен содержать только английские буквы, цифры и спецсимволы, без пробелов");
                    HighlightErrorField(txtPassword);
                    isValid = false;
                }
            }

            if (!isValid) MessageBox.Show(errors.ToString(), "Ошибки ввода", MessageBoxButton.OK, MessageBoxImage.Warning);
            return isValid;
        }

        private void CheckField(string value, string fieldName, Control control, StringBuilder errors)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                errors.AppendLine($"• Поле '{fieldName}' обязательно");
                HighlightErrorField(control);
            }
        }

        private void HighlightErrorField(Control control)
        {
            control.BorderBrush = Brushes.Red;
            control.BorderThickness = new Thickness(1);
        }

        private void ResetFieldBorders()
        {
            Control[] fields = { txtFirstName, txtLastName, txtSurname, txtEmail, txtPhone, txtPassword, txtConfirmPassword };
            foreach (var field in fields)
            {
                field.BorderBrush = Brushes.Gray;
                field.BorderThickness = new Thickness(1);
            }
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

        private async Task<bool> IsEmailExists(string email)
        {
            var users = await _unitOfWork.Users.FindAsync(u => u.Email == email);
            return users.Any();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            this.Close();
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordPlaceholder.Visibility = string.IsNullOrEmpty(txtPassword.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void txtConfirmPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            confirmPasswordPlaceholder.Visibility = string.IsNullOrEmpty(txtConfirmPassword.Password) ? Visibility.Visible : Visibility.Collapsed;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}