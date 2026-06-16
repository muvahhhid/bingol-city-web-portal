using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyCityProject.Models;

namespace MyCityProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<User, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Наши сущности
        public DbSet<Booking> Bookings { get; set; } = null!;
        public DbSet<District> Districts { get; set; } = null!;
        public DbSet<Population> Populations { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Ваши доп. настройки Fluent API при необходимости
        }
    }
}
