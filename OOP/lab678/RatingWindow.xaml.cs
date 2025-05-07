using System.Windows;

namespace RouteBookingSystem
{
    public partial class RatingWindow : Window
    {
        private readonly string _email;
        private readonly int _purchaseId;

        public RatingWindow(string email, int purchaseId)
        {
            InitializeComponent();
            _email = email;
            _purchaseId = purchaseId;
            this.Icon = new System.Windows.Media.Imaging.BitmapImage(new Uri("C:/BSTU/SEM4/OOP/lab4wpf5oop/lab4wpf5oop/Images/bus_icon-icons.com_76529.ico"));
        }

        private void SubmitRating_Click(object sender, RoutedEventArgs e)
        {
            int rating = RatingComboBox.SelectedIndex + 1;
            string comment = CommentTextBox.Text;

            var viewModel = new PurchaseHistoryViewModel(_email);
            viewModel.SaveRating(_purchaseId, rating, comment);

            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}