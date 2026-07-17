using Microsoft.EntityFrameworkCore;
using PortfolioApi.Models;

namespace PortfolioApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Note: no unique index on Username here — the free MySQL host used
        // for this project (freesqldatabase.com) fails to create unique
        // indexes with a "table is full" error due to its tiny quota.
        // Uniqueness is instead enforced in the AuthController when
        // updating credentials.
    }
}
