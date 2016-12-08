namespace AspNet_RssReader_Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Intial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Articles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Link = c.String(),
                        Description = c.String(),
                        Read = c.Boolean(nullable: false),
                        PubDate = c.DateTime(),
                        ImageUrl = c.String(),
                        SourceId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sources", t => t.SourceId, cascadeDelete: true)
                .Index(t => t.SourceId);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Link = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Articles", "SourceId", "dbo.Sources");
            DropIndex("dbo.Articles", new[] { "SourceId" });
            DropTable("dbo.Sources");
            DropTable("dbo.Articles");
        }
    }
}
