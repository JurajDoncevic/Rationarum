using Rationarum_v3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.ViewModels
{
    public class OutgoingInvoiceViewModel:InvoiceViewModel
    {

        [Required(ErrorMessage = "Podaci o kupcu su obavezni!")]
        [StringLength(512)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Podaci o kupcu")]
        public string CustomerInfo { get; set; }



        [Required(ErrorMessage = "Iznos je obavezan!")]
        [Display(Name = "Ukupni iznos (s PDV-om)")]
        [RegularExpression(@"[0-9]{1,8}\,[0-9]{1,2}", ErrorMessage = "Iznos mora biti sveden na dvije decimale (koristiti decimalni zarez)")]
        public string Amount { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}