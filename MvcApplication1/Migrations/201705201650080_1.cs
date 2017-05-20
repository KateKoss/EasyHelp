namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Profile", "articleName");
            DropColumn("dbo.Profile", "articleText");
            DropColumn("dbo.Profile", "TextSearhAssoasiates");
            DropColumn("dbo.Profile", "TextSearhAssistants");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profile", "TextSearhAssistants", c => c.String());
            AddColumn("dbo.Profile", "TextSearhAssoasiates", c => c.String());
            AddColumn("dbo.Profile", "articleText", c => c.String());
            AddColumn("dbo.Profile", "articleName", c => c.String());
        }
    }
}
