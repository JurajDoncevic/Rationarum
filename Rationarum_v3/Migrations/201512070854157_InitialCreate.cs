namespace Rationarum_v3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Expenditures",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        JournalEntryNum = c.String(maxLength: 128, unicode: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        AmountCash = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountTransferAccount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountNonCashBenefit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValueAddedTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Article22 = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Totaled = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        UserName = c.String(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        Email = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                        User_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.LoginProvider, t.ProviderKey })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.IngoingInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceClassNumber = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        SupplierInfo = c.String(nullable: false, maxLength: 512, unicode: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.OutgoingInvoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        InvoiceClassNumber = c.String(nullable: false),
                        Date = c.DateTime(nullable: false),
                        CustomerInfo = c.String(nullable: false, maxLength: 512, unicode: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
            CreateTable(
                "dbo.Receipts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        JournalEntryNum = c.String(maxLength: 128, unicode: false),
                        ApplicationUserId = c.String(nullable: false, maxLength: 128),
                        AmountCash = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountTransferAccount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        AmountNonCashBenefit = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ValueAddedTax = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Totaled = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Receipts", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.OutgoingInvoices", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.IngoingInvoices", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Expenditures", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Receipts", new[] { "ApplicationUserId" });
            DropIndex("dbo.OutgoingInvoices", new[] { "ApplicationUserId" });
            DropIndex("dbo.IngoingInvoices", new[] { "ApplicationUserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "User_Id" });
            DropIndex("dbo.Expenditures", new[] { "ApplicationUserId" });
            DropTable("dbo.Receipts");
            DropTable("dbo.OutgoingInvoices");
            DropTable("dbo.IngoingInvoices");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Expenditures");
        }
    }
}
