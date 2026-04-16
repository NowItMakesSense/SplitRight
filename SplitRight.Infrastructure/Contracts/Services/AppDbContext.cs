using Microsoft.EntityFrameworkCore;
using SplitRight.Domain.Contracts.Entities;
using SplitRight.Infrastructure.Contracts.Interfaces;

namespace SplitRight.Infrastructure.Contracts.Services
{
    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();

        public DbSet<UserRefreshToken> UsersRefreshToken => Set<UserRefreshToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
