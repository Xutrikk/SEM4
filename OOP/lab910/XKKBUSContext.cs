using Microsoft.EntityFrameworkCore;
using lab4wpf5oop.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace RouteBookingSystem.Data
{
    public class XKKBUSContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<PurchasedTicket> PurchasedTickets { get; set; }
        public DbSet<Favorite> Favorites { get; set; }
        public DbSet<TripRating> TripRatings { get; set; }

        public XKKBUSContext(DbContextOptions<XKKBUSContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка первичных и внешних ключей
            modelBuilder.Entity<User>()
                .HasKey(u => u.Email);

            modelBuilder.Entity<Ticket>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<PurchasedTicket>()
                .HasKey(pt => pt.PurchaseId);

            modelBuilder.Entity<Favorite>()
                .HasKey(f => f.Id);

            modelBuilder.Entity<TripRating>()
                .HasKey(tr => tr.Id);

            modelBuilder.Entity<PurchasedTicket>()
                .HasOne(pt => pt.User)
                .WithMany()
                .HasForeignKey(pt => pt.EmailUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchasedTicket>()
                .HasOne(pt => pt.Ticket)
                .WithMany()
                .HasForeignKey(pt => pt.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PurchasedTicket>()
        .HasOne(pt => pt.TripRating)
        .WithOne(tr => tr.PurchasedTicket)
        .HasForeignKey<TripRating>(tr => tr.PurchaseId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.EmailUser)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Ticket)
                .WithMany()
                .HasForeignKey(f => f.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TripRating>()
    .HasOne(tr => tr.PurchasedTicket) // TripRating ссылается на PurchasedTicket
    .WithOne(pt => pt.TripRating)      // PurchasedTicket имеет один TripRating
    .HasForeignKey<TripRating>(tr => tr.PurchaseId) // Внешний ключ в TripRating
    .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<TripRating>()
                .HasOne(tr => tr.User)
                .WithMany()
                .HasForeignKey(tr => tr.EmailUser);
        }
    }
}