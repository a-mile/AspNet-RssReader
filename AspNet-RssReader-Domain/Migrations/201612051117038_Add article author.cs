namespace AspNet_RssReader_Domain.Migrations
{
    using System.Data.Entity.Migrations;
    
    public partial class Addarticleauthor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Articles", "Author", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Articles", "Author");
        }
    }
}
