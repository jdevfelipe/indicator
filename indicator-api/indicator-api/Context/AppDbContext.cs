using indicator_api.Entities;
using Microsoft.EntityFrameworkCore;

namespace indicator_api.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Indication> Indications { get; set; }
        public DbSet<Indicator> Indicators { get; set; }
        public DbSet<PaymentAccount> PaymentAccounts { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Payment> Payments { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserCompany>()
                .HasKey(t => new { t.UserId, t.CompanyId });
        }
    }
}
