using Microsoft.EntityFrameworkCore;
using OurTime.WebUI.Models.Entities;

namespace OurTime.WebUI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        { }

        // Egen produkt-tabell
        public DbSet<Watch> Watches { get; set; }

        // Anropar externt review-API, ingen lokal Reviews DbSet längre
        // public DbSet<Review> Reviews { get; set; }

        // Används för användarhantering
        public DbSet<User> Users { get; set; }
    }
}
