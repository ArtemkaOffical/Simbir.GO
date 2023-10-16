using Microsoft.EntityFrameworkCore;
using Simbir.GO.Domain.Models;

namespace Simbir.GO.Infrastructure.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> opt) : base(opt)
        {
            Database.EnsureCreated();
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transport> Transports { get; set; }
        public DbSet<Rent> Rents { get; set; }
        public DbSet<Token> Tokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transport>()
                 .Property(e => e.TransportType)
                 .HasConversion(
                     v => v.ToString(),
                     v => (TransportType)Enum.Parse(typeof(TransportType), v));
            modelBuilder.Entity<Rent>()
                .Property(e => e.Type)
                .HasConversion(
                    v => v.ToString(),
                    v => (RentType)Enum.Parse(typeof(RentType), v));
        }
    }
}
