namespace AspNet_RssReader_Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SourcecategoryFK : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Sources", name: "Category_Id", newName: "CategoryId");
            RenameIndex(table: "dbo.Sources", name: "IX_Category_Id", newName: "IX_CategoryId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Sources", name: "IX_CategoryId", newName: "IX_Category_Id");
            RenameColumn(table: "dbo.Sources", name: "CategoryId", newName: "Category_Id");
        }
    }
}
