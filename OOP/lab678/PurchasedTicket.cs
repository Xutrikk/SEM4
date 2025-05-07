namespace lab4wpf5oop.Models
{
    public class PurchasedTicket
    {
        public int PurchaseId { get; set; }
        public string EmailUser { get; set; }
        public int TicketId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; } // Добавлено
        public string Time { get; set; } // Добавлено
        public double Price { get; set; }
        public int Number { get; set; }
        public int Status { get; set; }
        public string StatusText => Status == 0 ? "Active" : "Cancelled";
        public string Type { get; set; }
        public string BoardingPoints { get; set; }
        public string DropOffPoints { get; set; }
    }
}