namespace AspNet_RssReader_Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Deleterole : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetRoles", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetRoles", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
    }
}
