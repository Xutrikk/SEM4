using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab4wpf5oop.Models
{
    public class TripRating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("PurchasedTicket")]
        public int PurchaseId { get; set; }

        [ForeignKey("User")]
        public string EmailUser { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        public string Comment { get; set; }
        public DateTime RatingDate { get; set; }

        public PurchasedTicket PurchasedTicket { get; set; }
        public User User { get; set; }
    }
}