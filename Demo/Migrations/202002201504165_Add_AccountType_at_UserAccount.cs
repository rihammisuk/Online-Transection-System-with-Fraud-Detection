namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_AccountType_at_UserAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersAccounts", "AccountTypeForOne", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsersAccounts", "AccountTypeForOne");
        }
    }
}
