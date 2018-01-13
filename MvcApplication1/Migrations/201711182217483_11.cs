namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _11 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profile", "Tegs", c => c.String());
            AddColumn("dbo.Profile", "WhoRates", c => c.String());
            DropColumn("dbo.Profile", "MyTegs");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Profile", "MyTegs", c => c.String());
            DropColumn("dbo.Profile", "WhoRates");
            DropColumn("dbo.Profile", "Tegs");
        }
    }
}
