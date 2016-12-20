using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace Rationarum_v3.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Adress { get; set; }

        public string AssociationName { get; set; }

        public string OIB { get; set; }

        public string Email { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Expenditure> Expenditures { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<OutgoingInvoice> OutgoingInvoices { get; set; }

        public DbSet<IngoingInvoice> IngoingInvoices { get; set; }

        public DbSet<FixedAsset> FixedAssets { get; set; }

    }
}