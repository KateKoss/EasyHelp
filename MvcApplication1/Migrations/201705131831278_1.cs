namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Profile",
                c => new
                    {
                        UserName = c.String(nullable: false, maxLength: 128),
                        articleName = c.String(),
                        articleText = c.String(),
                        About_me = c.String(),
                        Name = c.String(),
                        UserPhoto = c.Binary(),
                        MyTegs = c.String(),
                        TextSearhAssoasiates = c.String(),
                        TextSearhAssistants = c.String(),
                    })
                .PrimaryKey(t => t.UserName);
            
            DropTable("dbo.Profile");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(),
                        About_me = c.String(),
                    })
                .PrimaryKey(t => t.UserId);
            
            DropTable("dbo.Profile");
        }
    }
}
