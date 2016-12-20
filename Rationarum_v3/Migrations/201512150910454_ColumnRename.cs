namespace Rationarum_v3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColumnRename : DbMigration
    {
        public override void Up()
        {
            RenameColumn("dbo.Expenditures", "Id", "IdExpenditure");
            RenameColumn("dbo.Expenditures", "Date", "DateExpenditure");

            RenameColumn("dbo.Receipts", "Id", "IdReceipt");
            RenameColumn("dbo.Receipts", "Date", "DateReceipt");

            RenameColumn("dbo.OutgoingInvoices", "Id", "IdOutgoingInvoice");
            RenameColumn("dbo.OutgoingInvoices", "Date", "DateOutgoingInvoice");

            RenameColumn("dbo.IngoingInvoices", "Id", "IdIngoingInvoice");
            RenameColumn("dbo.IngoingInvoices", "Date", "DateIngoingInvoice");
        }
        
        public override void Down()
        {

        }
    }
}
