using Microsoft.EntityFrameworkCore;

namespace UNPChecker.Models
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        
        public DbSet<UNP> users { get; set; }
    }
}