namespace AspNet_RssReader_Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Sourcesyncdate : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Sources", new[] { "Name" });
            AddColumn("dbo.Sources", "SyncDate", c => c.DateTime());
            AlterColumn("dbo.Sources", "Name", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Sources", "Name", c => c.String(maxLength: 56));
            DropColumn("dbo.Sources", "SyncDate");
            CreateIndex("dbo.Sources", "Name");
        }
    }
}
