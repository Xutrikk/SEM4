using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace lab4wpf5oop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent(); // Автоматически сгенерированный метод

            // Установка иконки и курсора
            try
            {
                //this.Cursor = new Cursor(@"C:\\BSTU\\SEM4\\OOP\\lab4wpf5oop\\lab4wpf5oop\\Images\\free-icon-precision-12374267.cur");
                this.Icon = new BitmapImage(new Uri("C:/BSTU/SEM4/OOP/lab4wpf5oop/lab4wpf5oop/Images/bus_icon-icons.com_76529.ico"));
            
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки ресурсов: {ex.Message}");
            }

            // Проверка подключения к БД
            CheckDatabaseConnection();
        }

  

        private void CheckDatabaseConnection()
        {
            try
            {
                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["XKKBUS"].ConnectionString))
                {
                    conn.Open();
                    MessageBox.Show("Подключение к БД успешно!");
                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения: {ex.Message}");
            }
        }



        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика поиска маршрутов
        }

        private void BuyTicket_Click(object sender, RoutedEventArgs e)
        {
            // Логика покупки билета
        }
    }
}