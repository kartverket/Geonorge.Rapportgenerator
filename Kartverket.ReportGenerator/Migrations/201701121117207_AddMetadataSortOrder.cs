namespace Kartverket.ReportGenerator.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMetadataSortOrder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MetadataEntries", "SortOrder", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.MetadataEntries", "SortOrder");
        }
    }
}
