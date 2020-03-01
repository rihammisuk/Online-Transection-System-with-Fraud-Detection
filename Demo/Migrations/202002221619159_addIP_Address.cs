namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addIP_Address : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersAccounts", "UserIPAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsersAccounts", "UserIPAddress");
        }
    }
}
