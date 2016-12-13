namespace BuildSecureSystems.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _03_AddedApiKeys : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApiKeys",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Description = c.String(),
                        Permissions = c.String(),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApiKeys", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.ApiKeys", new[] { "ApplicationUser_Id" });
            DropTable("dbo.ApiKeys");
        }
    }
}
