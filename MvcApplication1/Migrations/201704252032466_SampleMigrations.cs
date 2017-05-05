using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.Entity.Migrations;

namespace MvcApplication1.Migrations
{


    public partial class SampleMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfile",
                c => new
                {
                    UserId = c.Int(nullable: false),
                    UserName = c.String(nullable: true, maxLength: 128),
                    About_me = c.String(),
                })
                .PrimaryKey(t => t.UserId);
        }

        public override void Down()
        {
            DropTable("dbo.UserProfile");
        }
    }
}