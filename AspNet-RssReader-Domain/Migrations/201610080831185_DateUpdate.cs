namespace AspNet_RssReader_Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class DateUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Articles", "PubDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Articles", "PubDate", c => c.DateTime());
        }
    }
}
