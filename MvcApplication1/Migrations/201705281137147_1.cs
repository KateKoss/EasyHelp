namespace MvcApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FindAssosiates",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        user = c.String(),
                        questionId = c.Int(nullable: false),
                        valueOfanswer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.FindAssosiates");
        }
    }
}
