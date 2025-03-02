using Microsoft.EntityFrameworkCore;

namespace LTW_Nhóm8386_FriendlyURL.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<UrlShortener> UrlShorteners { get; set; }
    }
}
