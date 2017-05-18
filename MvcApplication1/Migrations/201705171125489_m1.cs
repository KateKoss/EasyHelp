namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserProfile", "Role", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UserProfile", "Role");
        }
    }
}
