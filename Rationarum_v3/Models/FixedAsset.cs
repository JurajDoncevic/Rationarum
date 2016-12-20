using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.Models
{
    public class FixedAsset
    {
        [Key]
        public int IdFixedAsset { get; set; }


        [Required(ErrorMessage = "Naziv stvari ili prava je obavezan!")]
        [StringLength(128)]
        [Display(Name = "Naziv stvari ili prava")]
        public string NameFixedAsset { get; set; }


        [Required(ErrorMessage = "Datum nabave je obavezan!")]
        [Display(Name = "Datum nabave")]
        [DataType(DataType.Date)]
        public DateTime DateFixedAsset { get; set; }


        [Required(ErrorMessage = "Broj isprave je obavezan!")]
        [StringLength(128)]
        [Display(Name = "Broj isprave")]
        public string JournalEntryNum { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [Required(ErrorMessage = "Nabavna vrijednost je obavezna!")]
        [Display(Name = "Nabavna vrijednost")]
        [DataType(DataType.Currency)]
        public decimal PurchaseValue { get; set; }

        [Required(ErrorMessage = "Knjigovodstvena vrijednost je obavezna!")]
        [Display(Name = "Knjigovodstvena vrijednost")]
        [DataType(DataType.Currency)]
        public decimal BookValue { get; set; }

        [Required(ErrorMessage = "Vijek trajanja je obavezan podatak")]
        [StringLength(128)]
        [Display(Name = "Vijek trajanja")]
        public string Lifetime { get; set; }

        [Required(ErrorMessage = "Stopa otpisa je obavezna!")]
        [Display(Name = "Stopa otpisa")]
        [RegularExpression(@"(0|[1-9][0-9]{0,2})(\,[0-9][0-9]?)?$", ErrorMessage = "Stopa se izražava u postocima bez znaka %")]
        public decimal WriteDownRate { get; set; }

        [Required(ErrorMessage = "Svota otpisa je obavezna!")]
        [Display(Name = "Svota otpisa")]
        [DataType(DataType.Currency)]
        public decimal WriteDownValue { get; set; }

        [Required(ErrorMessage = "Knjigovodstvena vrijednost na kraju godine je obavezna!")]
        [Display(Name = "Vrijednost na kraju godine")]
        [DataType(DataType.Currency)]
        public decimal BookValueAtYearEnd { get; set; }


    }
}