namespace Kartverket.ReportGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatisticalData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Statistics",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Organization = c.String(maxLength: 255),
                        Measurement = c.String(maxLength: 255),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Date)
                .Index(t => t.Organization)
                .Index(t => t.Measurement)
                .Index(t => t.Count);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.Statistics", new[] { "Count" });
            DropIndex("dbo.Statistics", new[] { "Measurement" });
            DropIndex("dbo.Statistics", new[] { "Organization" });
            DropIndex("dbo.Statistics", new[] { "Date" });
            DropTable("dbo.Statistics");
        }
    }
}
