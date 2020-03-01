namespace Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class gender1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "FullName", c => c.String());
        }
    }
}
