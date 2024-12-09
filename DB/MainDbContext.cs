using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace GSI_WebApi.DB
{
    public class MainDbContext : DbContext
    {
        public MainDbContext() : base()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=192.168.1.229;Database=users;User Id=GSI26178;Password=teste123!\"#;TrustServerCertificate=True");
        }

        public virtual DbSet<Employee> Employees { get; set; }
    }
}