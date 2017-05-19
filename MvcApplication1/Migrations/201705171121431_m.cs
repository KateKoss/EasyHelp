namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "EmailAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "EmailAddress");
        }
    }
}
