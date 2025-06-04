using lab4wpf5oop.Models;
using Microsoft.EntityFrameworkCore;
using RouteBookingSystem.Data;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RouteBookingSystem
{
    public partial class LoginWindow : Window
    {
        private readonly IUnitOfWork _unitOfWork;

        public LoginWindow()
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("C:/BSTU/SEM4/OOP/lab4wpf5oop/lab4wpf5oop/Images/bus_icon-icons.com_76529.ico"));
            var options = new DbContextOptionsBuilder<XKKBUSContext>()
                .UseSqlServer("Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;TrustServerCertificate=True;")
                .Options;
            _unitOfWork = new UnitOfWork(new XKKBUSContext(options));
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите email и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!IsValidEmail(email))
            {
                MessageBox.Show("Неверный формат Email (только английские буквы и цифры, без спецсимволов (кроме @ и .), без пробелов)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!IsValidPassword(password))
            {
                MessageBox.Show("Неверный формат пароля (минимум 6 символов, только английские буквы, цифры и спецсимволы, без пробелов)", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                var user = (await _unitOfWork.Users.FindAsync(u => u.Email == email)).FirstOrDefault();
                if (user != null)
                {
                    if (!user.IsAdmin)
                    {
                        MessageBox.Show("Вход разрешён только для администраторов!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (user.IsBlocked)
                    {
                        MessageBox.Show("Ваш аккаунт заблокирован!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (VerifyPassword(password, user.PasswordHash))
                    {
                        MessageBox.Show("Вход выполнен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        var adminWindow = new AdminWindow(user);
                        adminWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Неверный пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Пользователь не найден!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool VerifyPassword(string inputPassword, string storedHash)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(inputPassword);
                byte[] hashedBytes = sha256.ComputeHash(inputBytes);
                string hashedInput = Convert.ToBase64String(hashedBytes);
                return hashedInput == storedHash;
            }
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[a-zA-Z][a-zA-Z0-9]*@[a-zA-Z][a-zA-Z0-9]*\.[a-zA-Z]+$");
        }

        private bool IsValidPassword(string password)
        {
            return !string.IsNullOrEmpty(password) && password.Length >= 6 &&
                   Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()+\-=\[\]{};:'"",.<>?]+$") &&
                   !password.Contains(" ");
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            var registrationWindow = new RegistrationWindow();
            registrationWindow.Show();
            this.Close();
        }

        private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtEmailPlaceholder != null)
            {
                txtEmailPlaceholder.Visibility = string.IsNullOrEmpty(txtEmail.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPasswordPlaceholder != null)
            {
                txtPasswordPlaceholder.Visibility = string.IsNullOrEmpty(txtPassword.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}