using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.Models
{
    public class IngoingInvoice
    {
        [Key]
        public int IdIngoingInvoice { get; set; }


        [Required(ErrorMessage = "Broj računa je obavezan!")]
        [Display(Name = "Broj računa")]
        public string InvoiceClassNumber { get; set; }


        [Required(ErrorMessage = "Datum je obavezan!")]
        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        public DateTime DateIngoingInvoice { get; set; }


        [Required(ErrorMessage = "Podaci o dobavljaču su obavezni!")]
        [StringLength(512)]
        [Display(Name = "Podaci o dobavljaču")]
        public string SupplierInfo { get; set; }


        [Display(Name = "Ukupni iznos (s PDV-om)")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}