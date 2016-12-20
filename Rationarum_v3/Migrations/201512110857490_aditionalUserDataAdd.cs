namespace Rationarum_v3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class aditionalUserDataAdd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Adress", c => c.String());
            AddColumn("dbo.AspNetUsers", "AssociationName", c => c.String());
            AddColumn("dbo.AspNetUsers", "OIB", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "OIB");
            DropColumn("dbo.AspNetUsers", "AssociationName");
            DropColumn("dbo.AspNetUsers", "Adress");
        }
    }
}
