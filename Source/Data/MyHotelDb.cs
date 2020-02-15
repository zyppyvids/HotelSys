using Data.Entity;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MyHotelDb : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\ProjectsV13;Initial Catalog=MyHotelDb;");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
