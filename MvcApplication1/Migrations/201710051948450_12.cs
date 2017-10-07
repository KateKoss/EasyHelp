namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatMasseges",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FromUser = c.String(),
                        ToUser = c.String(),
                        Massege = c.String(),
                        DateTimeSent = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ChatMasseges");
        }
    }
}
