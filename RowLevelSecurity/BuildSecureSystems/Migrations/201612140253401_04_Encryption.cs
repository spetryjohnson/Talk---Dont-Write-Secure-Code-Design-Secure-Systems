namespace BuildSecureSystems.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _04_Encryption : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CreditCardNumber", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CreditCardNumber");
        }
    }
}
