using DapperDemoWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace DapperDemoWebApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {            
        }

        public DbSet<Company> Companies { get; set; }
    }
}
