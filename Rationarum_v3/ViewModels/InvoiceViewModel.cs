using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rationarum_v3.ViewModels
{
    public abstract class InvoiceViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Broj računa je obavezan!")]
        [Display(Name = "Broj računa")]
        public string InvoiceClassNumber { get; set; }


        [Required(ErrorMessage = "Datum je obavezan!")]
        [Display(Name = "Datum")]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])[.]([1-9]|1[012])[.](19|20)\d\d\.$", ErrorMessage = "Datum nije u obliku d.m.gggg.")]
        public string Date { get; set; }
    }
}