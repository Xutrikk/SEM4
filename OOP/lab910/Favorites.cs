using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace lab4wpf5oop.Models
{
    public class Favorite
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public string EmailUser { get; set; }

        [ForeignKey("Ticket")]
        public int TicketId { get; set; }

        public DateTime AddedDate { get; set; }

        public User User { get; set; }
        public Ticket Ticket { get; set; }
    }
}