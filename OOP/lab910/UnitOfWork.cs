using lab4wpf5oop.Models;

namespace RouteBookingSystem.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly XKKBUSContext _context;
        private readonly IRepository<User> _users;
        private readonly IRepository<Ticket> _tickets;
        private readonly IRepository<PurchasedTicket> _purchasedTickets;
        private readonly IRepository<Favorite> _favorites;
        private readonly IRepository<TripRating> _tripRatings;

        public UnitOfWork(XKKBUSContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _users = new Repository<User>(context);
            _tickets = new Repository<Ticket>(context);
            _purchasedTickets = new Repository<PurchasedTicket>(context);
            _favorites = new Repository<Favorite>(context);
            _tripRatings = new Repository<TripRating>(context);
        }

        public XKKBUSContext Context => _context;

        public IRepository<User> Users => _users;
        public IRepository<Ticket> Tickets => _tickets;
        public IRepository<PurchasedTicket> PurchasedTickets => _purchasedTickets;
        public IRepository<Favorite> Favorites => _favorites;
        public IRepository<TripRating> TripRatings => _tripRatings;

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}