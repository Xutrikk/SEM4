using lab4wpf5oop.Models;
using Microsoft.EntityFrameworkCore;
using RouteBookingSystem.Data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RouteBookingSystem
{
    public partial class AdminWindow : Window
    {
        private readonly IUnitOfWork _unitOfWork;
        private bool _disposed = false;

        public AdminWindow(User currentAdmin)
        {
            InitializeComponent();
            this.Icon = new BitmapImage(new Uri("C:/BSTU/SEM4/OOP/lab4wpf5oop/lab4wpf5oop/Images/bus_icon-icons.com_76529.ico"));

            try
            {
                var options = new DbContextOptionsBuilder<XKKBUSContext>()
                    .UseSqlServer("Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;TrustServerCertificate=True;")
                    .Options;
                var context = new XKKBUSContext(options);
                _unitOfWork = new UnitOfWork(context);

                InitializeViewModelAsync(currentAdmin);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации AdminWindow: {ex.Message}\nПодробности: {ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private async void InitializeViewModelAsync(User currentAdmin)
        {
            try
            {
                var viewModel = new AdminViewModel(_unitOfWork, currentAdmin);
                DataContext = viewModel;
                await viewModel.InitializeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных в AdminWindow: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }
        }

        private void txtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminViewModel vm)
            {
                var passwordBox = (PasswordBox)sender;
                vm.SelectedUserPassword = passwordBox.Password;
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Dispose();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _unitOfWork?.Dispose();
                _disposed = true;
            }
        }
    }
}