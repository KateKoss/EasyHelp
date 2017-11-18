namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _13 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Profile", "Rates", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Profile", "Rates");
        }
    }
}
