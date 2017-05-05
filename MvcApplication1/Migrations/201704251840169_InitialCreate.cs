namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProfileModels",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        About_me = c.String(),
                    })
                .PrimaryKey(t => t.UserName);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProfileModels");
        }
    }
}
