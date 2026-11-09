using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using University_Cafeteria.Models;

namespace University_Cafeteria.Data
{
    public class CafeteriaDbContext : DbContext
    {

        public CafeteriaDbContext(DbContextOptions<CafeteriaDbContext> options) : base(options)
        {
        }


        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Order> Orders { get; set; }


    }
}
