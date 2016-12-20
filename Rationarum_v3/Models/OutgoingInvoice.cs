using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.Models
{
    public class OutgoingInvoice
    {
        [Key]
        public int IdOutgoingInvoice { get; set; }


        [Required(ErrorMessage = "Broj računa je obavezan!")]
        [Display(Name = "Broj računa")]
        public string InvoiceClassNumber { get; set; }


        [Required(ErrorMessage = "Datum je obavezan!")]
        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        public DateTime DateOutgoingInvoice { get; set; }


        [Required(ErrorMessage = "Podaci o kupcu su obavezni!")]
        [StringLength(512)]
        [Display(Name = "Podaci o kupcu")]
        public string CustomerInfo { get; set; }


        
        [Required(ErrorMessage = "Iznos je obavezan!")]
        [Display(Name = "Ukupni iznos (s PDV-om)")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }


    }
}