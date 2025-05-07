using lab4wpf5oop;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace RouteBookingSystem
{
    public partial class LoginWindow : Window
    {
        private readonly string connectionString = "Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;";

        public LoginWindow()
        {
            InitializeComponent();
            //this.Cursor = new Cursor(@"C:\\BSTU\\SEM4\\OOP\\lab4wpf5oop\\lab4wpf5oop\\Images\\free-icon-precision-12374267.cur");
            this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/BSTU/SEM4/OOP/lab4wpf5oop/lab4wpf5oop/Images/bus_icon-icons.com_76529.ico"));
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Введите email и пароль!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT PasswordHash, isAdmin FROM Users WHERE Email = @Email";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHash = reader.GetString(0);
                                bool isAdmin = reader.GetBoolean(1);

                                if (VerifyPassword(password, storedHash))
                                {
                                    MessageBox.Show("Вход выполнен успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                                    OpenUserWindow(isAdmin);
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
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к базе данных: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenUserWindow(bool isAdmin)
        {
            Window nextWindow = isAdmin ? new AdminWindow() : new UserDashboardWindow(txtEmail.Text.Trim());
            nextWindow.Show();
            this.Close();
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
                txtEmailPlaceholder.Visibility =
                    string.IsNullOrEmpty(txtEmail.Text) ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        // Обработчик изменения пароля
        private void txtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtPasswordPlaceholder != null)
            {
                txtPasswordPlaceholder.Visibility =
                    string.IsNullOrEmpty(txtPassword.Password) ? Visibility.Visible : Visibility.Collapsed;
            }
        }
        private void RussianButton_Click(object sender, RoutedEventArgs e)
        {
            LocalizationManager.SwitchLanguage("ru-RU");
        }

        private void EnglishButton_Click(object sender, RoutedEventArgs e)
        {
            LocalizationManager.SwitchLanguage("en-US");
        }
    }
}