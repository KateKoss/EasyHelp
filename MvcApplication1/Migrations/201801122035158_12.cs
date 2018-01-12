namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Article", "dateOfLastEditing", c => c.DateTime(nullable: false));
            AddColumn("dbo.Article", "dateOfCreation", c => c.DateTime(nullable: false));
            AddColumn("dbo.Article", "tagList", c => c.String());
            AlterColumn("dbo.Article", "articleTitle", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Article", "articleTitle", c => c.String(nullable: false));
            DropColumn("dbo.Article", "tagList");
            DropColumn("dbo.Article", "dateOfCreation");
            DropColumn("dbo.Article", "dateOfLastEditing");
        }
    }
}
