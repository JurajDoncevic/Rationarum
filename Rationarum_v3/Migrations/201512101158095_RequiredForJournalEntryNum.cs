namespace Rationarum_v3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RequiredForJournalEntryNum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Expenditures", "JournalEntryNum", c => c.String(nullable: false, maxLength: 128, unicode: false));
            AlterColumn("dbo.Receipts", "JournalEntryNum", c => c.String(nullable: false, maxLength: 128, unicode: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Receipts", "JournalEntryNum", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.Expenditures", "JournalEntryNum", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
