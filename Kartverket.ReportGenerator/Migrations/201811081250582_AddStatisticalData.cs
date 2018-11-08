namespace Kartverket.ReportGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStatisticalData : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Data",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Content = c.String(),
                        Organization = c.String(),
                        Count = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Data");
        }
    }
}
