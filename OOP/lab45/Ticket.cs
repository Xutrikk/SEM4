using System;

namespace RouteBookingSystem
{
    public class Ticket
    {
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }
        public string Time { get; set; }
        public double Price { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}