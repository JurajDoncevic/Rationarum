using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.Models
{
    public class Expenditure
    {
        [Key]
        public int IdExpenditure { get; set; }


        [Required(ErrorMessage = "Datum je obavezan!")]
        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        public DateTime DateExpenditure { get; set; }


        [Required(ErrorMessage = "Broj temeljnice i opis dokumenta su obavezni!")]
        [StringLength(128)]
        [Display(Name = "Broj temeljnice/Opis dokumenta")]
        public string JournalEntryNum { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }


        [Required(ErrorMessage = "Iznos u gotovini je obavezan!")]
        [Display(Name = "U gotovini")]
        [DataType(DataType.Currency)]
        public decimal AmountCash { get; set; }


        [Required(ErrorMessage = "Iznos na žiro račun je obavezan!")]
        [Display(Name = "Na žiro-račun")]
        [DataType(DataType.Currency)]
        public decimal AmountTransferAccount { get; set; }


        [Required(ErrorMessage = "Iznos u naravi obavezan!")]
        [Display(Name = "U naravi")]
        [DataType(DataType.Currency)]
        public decimal AmountNonCashBenefit { get; set; }


        [Required(ErrorMessage = "Iznos u PDV je obavezan!")]
        [Display(Name = "Iznos PDV-a")]
        [DataType(DataType.Currency)]
        public decimal ValueAddedTax { get; set; }


        [Required(ErrorMessage = "Iznos u članku 22 je obavezan!")]
        [Display(Name = "Izdaci iz članka 22")]
        [DataType(DataType.Currency)]
        public decimal Article22 { get; set; }


        [Required]
        [Display(Name = "Ukupno")]
        [DataType(DataType.Currency)]
        public decimal Totaled { get; set; }
    }
}