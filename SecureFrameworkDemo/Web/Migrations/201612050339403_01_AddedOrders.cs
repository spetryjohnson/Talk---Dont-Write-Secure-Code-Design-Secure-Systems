namespace SecureFrameworkDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _01_AddedOrders : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.ApplicationUserPermissions", newName: "PermissionApplicationUsers");
            DropPrimaryKey("dbo.PermissionApplicationUsers");
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        PlacedOn = c.DateTime(nullable: false),
                        ApplicationUser_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_Id)
                .Index(t => t.ApplicationUser_Id);
            
            AddPrimaryKey("dbo.PermissionApplicationUsers", new[] { "Permission_Id", "ApplicationUser_Id" });
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Orders", new[] { "ApplicationUser_Id" });
            DropPrimaryKey("dbo.PermissionApplicationUsers");
            DropTable("dbo.Orders");
            AddPrimaryKey("dbo.PermissionApplicationUsers", new[] { "ApplicationUser_Id", "Permission_Id" });
            RenameTable(name: "dbo.PermissionApplicationUsers", newName: "ApplicationUserPermissions");
        }
    }
}
