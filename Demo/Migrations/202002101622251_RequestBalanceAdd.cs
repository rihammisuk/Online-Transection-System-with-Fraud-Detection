namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequestBalanceAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MoneyRequests", "RequestBalance", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.MoneyRequests", "RequestBalance");
        }
    }
}
