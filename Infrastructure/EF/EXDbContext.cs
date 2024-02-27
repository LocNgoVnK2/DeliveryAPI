using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EF
{
    public class EXDbContext : DbContext
    {
 
        public EXDbContext(DbContextOptions<EXDbContext> options) : base(options)
        {
        }
        public DbSet<CheckOut> checkOuts { get; set; }
        public DbSet<Accounts> accounts { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<DeliveryDetail> DeliveryDetails { get; set; }
    }
}
