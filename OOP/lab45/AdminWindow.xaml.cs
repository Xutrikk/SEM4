using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace RouteBookingSystem
{
    public partial class AdminWindow : Window
    {
        private readonly string connectionString = "Server=DESKTOP-DMJJTUE\\SQLEXPRESS;Database=XKKBUS;Trusted_Connection=True;";

        public AdminWindow()
        {
            InitializeComponent();
            this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/BSTU/SEM4/OOP/lab4wpf5oop/lab4wpf5oop/Images/bus_icon-icons.com_76529.ico"));
            DataContext = new AdminViewModel(connectionString);
        }

        private void txtNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is AdminViewModel vm && vm.SelectedUser != null)
            {
                var passwordBox = (PasswordBox)sender;
                vm.SelectedUser.PasswordHash = vm.HashPassword(passwordBox.Password);
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