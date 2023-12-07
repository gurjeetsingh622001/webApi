using Microsoft.EntityFrameworkCore;
using webApi.Models;
namespace webApi.NewFolder
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options) { }
        public DbSet<Employee> Employees { get; set; }
    }
}
