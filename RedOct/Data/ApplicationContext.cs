using Microsoft.EntityFrameworkCore;
using RedOct.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedOct.Data
{
    internal class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ProductionPlane> ProductionPlanes { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<PurchaseMaterial> PurchaseMaterials { get; set; }

        //public ApplicationContext()
        //{
        //    Database.EnsureCreated();
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-C4OPFC5;Database=RedOct;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
