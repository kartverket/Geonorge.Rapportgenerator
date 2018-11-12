using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Kartverket.ReportGenerator.Models
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext():base("DefaultConnection"){
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ReportDbContext, Kartverket.ReportGenerator.Migrations.Configuration>("DefaultConnection"));
        }

        public virtual DbSet<MetadataEntry> MetadataEntries { get; set; }
        public virtual DbSet<Statistics> StatisticalData { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Statistics>().HasIndex(i => i.Date);
            modelBuilder.Entity<Statistics>().HasIndex(i => i.Organization);
            modelBuilder.Entity<Statistics>().HasIndex(i => i.Measurement);
            modelBuilder.Entity<Statistics>().HasIndex(i => i.Count);
        }

    }
}