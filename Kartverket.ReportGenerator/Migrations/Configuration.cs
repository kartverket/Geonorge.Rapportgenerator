namespace Kartverket.ReportGenerator.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Kartverket.ReportGenerator.Models.ReportDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Kartverket.ReportGenerator.Models.ReportDbContext context)
        {
            //  This method will be called after migrating to the latest version.

        }
    }
}
