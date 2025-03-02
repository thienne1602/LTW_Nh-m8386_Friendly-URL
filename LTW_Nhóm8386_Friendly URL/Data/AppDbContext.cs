using LTW_Nhóm8386_FriendlyURL.Models;
using Microsoft.EntityFrameworkCore;

namespace LTW_Nhóm8386_FriendlyURL.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<URL> URLs { get; set; }
    }
}
