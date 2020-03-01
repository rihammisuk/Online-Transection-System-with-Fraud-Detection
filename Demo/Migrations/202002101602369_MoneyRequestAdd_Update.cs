namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoneyRequestAdd_Update : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.MoneyRequests", "RequestID", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.MoneyRequests", "RequestID", c => c.Int(nullable: false));
        }
    }
}
