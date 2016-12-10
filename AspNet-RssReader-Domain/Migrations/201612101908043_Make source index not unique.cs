namespace AspNet_RssReader_Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Makesourceindexnotunique : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sources", new[] { "Name" });
            RenameColumn(table: "dbo.Sources", name: "ApplcationUserId", newName: "ApplicationUserId");
            RenameIndex(table: "dbo.Sources", name: "IX_ApplcationUserId", newName: "IX_ApplicationUserId");
            AlterColumn("dbo.Sources", "Name", c => c.String(maxLength: 56));
            CreateIndex("dbo.Sources", "Name");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Sources", new[] { "Name" });
            AlterColumn("dbo.Sources", "Name", c => c.String(maxLength: 50));
            RenameIndex(table: "dbo.Sources", name: "IX_ApplicationUserId", newName: "IX_ApplcationUserId");
            RenameColumn(table: "dbo.Sources", name: "ApplicationUserId", newName: "ApplcationUserId");
            CreateIndex("dbo.Sources", "Name", unique: true);
        }
    }
}
