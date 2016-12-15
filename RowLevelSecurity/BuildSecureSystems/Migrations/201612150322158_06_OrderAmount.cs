namespace BuildSecureSystems.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _06_OrderAmount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "Amount", c => c.Decimal(nullable: false, precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "Amount");
        }
    }
}
