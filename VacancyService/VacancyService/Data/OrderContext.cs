using Microsoft.EntityFrameworkCore;
using RabbitDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VacancyService.Data
{
    public class VacancyContext : DbContext
    {
        public VacancyContext(DbContextOptions<VacancyContext> options) : base(options)
        {
        }

        public DbSet<VacancyItems> VacancyItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VacancyItems>().ToTable("VacancyItems");
        }
    }
}
