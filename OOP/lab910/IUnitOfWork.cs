using lab4wpf5oop.Models;

namespace RouteBookingSystem.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<User> Users { get; }
        IRepository<Ticket> Tickets { get; }
        IRepository<PurchasedTicket> PurchasedTickets { get; }
        IRepository<Favorite> Favorites { get; }
        IRepository<TripRating> TripRatings { get; }
        Task<int> SaveChangesAsync();
    }
}