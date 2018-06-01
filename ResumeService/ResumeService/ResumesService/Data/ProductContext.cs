using Microsoft.EntityFrameworkCore;
using RabbitDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ResumesService.Data
{
    public class ResumeContext : DbContext
    {
        public ResumeContext(DbContextOptions<ResumeContext> options) : base(options)
        {
        }

        public DbSet<Resume> Resumes { get; set; }
        public DbSet<Company> Categories { get; set; }
        public DbSet<Resume_Company> Resume_Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Resume>().ToTable("Resume");
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<Resume_Company>().ToTable("Resume_Company");
        }
    }
}
