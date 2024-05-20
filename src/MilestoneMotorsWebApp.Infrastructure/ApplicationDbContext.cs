using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilestoneMotorsWebApp.Domain.Entities;
using MilestoneMotorsWebApp.Domain.Enums;

namespace MilestoneMotorsWebApp.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public DbSet<Car> Cars { get; init; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<Car>()
                .Property(c => c.Brand)
                .HasConversion(c => c.ToString(), c => (Brand)Enum.Parse(typeof(Brand), c));

            builder
                .Entity<Car>()
                .HasOne(c => c.User)
                .WithMany(u => u.MyListings)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
