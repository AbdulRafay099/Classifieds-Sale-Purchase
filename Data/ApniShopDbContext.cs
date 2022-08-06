using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApniShop.Models;

namespace ApniShop.Data
{
    public class ApniShopDbContext : DbContext
    {
        public ApniShopDbContext(DbContextOptions<ApniShopDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<Product> Product { get; set; }

    }
}
