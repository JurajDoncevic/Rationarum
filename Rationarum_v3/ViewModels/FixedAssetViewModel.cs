using Rationarum_v3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rationarum_v3.ViewModels
{
    public class FixedAssetViewModel
    {
        
        public int Id { get; set; }


        [Required(ErrorMessage = "Naziv stvari ili prava je obavezan!")]
        [StringLength(128)]
        [Display(Name = "Naziv stvari ili prava")]
        public string Name { get; set; }


        [Required(ErrorMessage = "Datum nabave je obavezan!")]
        [Display(Name = "Datum nabave")]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])[.]([1-9]|1[012])[.](19|20)\d\d\.$", ErrorMessage = "Datum nije u obliku d.m.gggg.")]
        public string Date { get; set; }


        [Required(ErrorMessage = "Broj isprave je obavezan!")]
        [StringLength(128)]
        [Display(Name = "Broj isprave")]
        public string JournalEntryNum { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Nabavna vrijednost je obavezna!")]
        [Display(Name = "Nabavna vrijednost")]
        [RegularExpression(@"[0-9]{1,8}\,[0-9]{1,2}", ErrorMessage = "Iznos mora biti sveden na dvije decimale (koristiti decimalni zarez)")]
        public string PurchaseValue { get; set; }

        [Required(ErrorMessage = "Knjigovodstvena vrijednost je obavezna!")]
        [Display(Name = "Knjigovodstvena vrijednost")]
        [RegularExpression(@"[0-9]{1,8}\,[0-9]{1,2}", ErrorMessage = "Iznos mora biti sveden na dvije decimale (koristiti decimalni zarez)")]
        public string BookValue { get; set; }

        [Required(ErrorMessage = "Vijek trajanja je obavezan podatak")]
        [StringLength(128)]
        [Display(Name = "Vijek trajanja")]
        public string Lifetime { get; set; }

        [Required(ErrorMessage = "Stopa otpisa je obavezna!")]
        [Display(Name = "Stopa otpisa")]
        [RegularExpression(@"(0|[1-9][0-9]{0,2})(\,[0-9][0-9]?)?$", ErrorMessage = "Stopa se izražava u postocima bez znaka %")]
        public string WriteDownRate { get; set; }

        [Required(ErrorMessage = "Svota otpisa je obavezna!")]
        [Display(Name = "Svota otpisa")]
        [RegularExpression(@"[0-9]{1,8}\,[0-9]{1,2}", ErrorMessage = "Iznos mora biti sveden na dvije decimale (koristiti decimalni zarez)")]
        public string WriteDownValue { get; set; }

        [Required(ErrorMessage = "Knjigovodstvena vrijednost na kraju godine je obavezna!")]
        [Display(Name = "Vrijednost na kraju godine")]
        [RegularExpression(@"[0-9]{1,8}\,[0-9]{1,2}", ErrorMessage = "Iznos mora biti sveden na dvije decimale (koristiti decimalni zarez)")]
        public string BookValueAtYearEnd { get; set; }
    }
}