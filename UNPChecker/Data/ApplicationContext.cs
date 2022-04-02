using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
     => options.UseSqlServer("Server=DESKTOP-1OH4LVK\\SQLEXPRESS;Database=UNPDB;Trusted_Connection=True;MultipleActiveResultSets=true");

        public DbSet<UNP> users { get; set; }
    }
}