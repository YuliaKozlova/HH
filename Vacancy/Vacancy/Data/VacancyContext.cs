using vacancyService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace vacancyService.Data
{
    public class vacancyContext : DbContext
    {
        public vacancyContext(DbContextOptions<vacancyContext> options) : base(options)
        {

        }

        public DbSet<employer> Employer { get; set; }
        public DbSet<vacancy> vacancys { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<employer>().ToTable("employer");
            modelBuilder.Entity<vacancy>().ToTable("vacancy");
        }
    }
}
