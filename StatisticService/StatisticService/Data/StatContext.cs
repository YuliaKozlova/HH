using Microsoft.EntityFrameworkCore;
using RabbitDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StatisticService.Data
{
    public class StatContext : DbContext
    {
        public StatContext(DbContextOptions<StatContext> options) : base(options)
        {
        }

        public DbSet<Statistic> Statistic { get; set; }
        public DbSet<Statistic> StatisticFromQueue { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Statistic>().ToTable("Statistic");
            modelBuilder.Entity<Statistic>().ToTable("StatisticFromQueue");
        }
    }
}
