using System;
using System.ComponentModel.DataAnnotations;

namespace lab4wpf5oop.Models
{
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public double Price { get; set; }
        public int Number { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string BoardingPoints { get; set; }
        public string DropOffPoints { get; set; }
        public string Company { get; set; }
    }
}