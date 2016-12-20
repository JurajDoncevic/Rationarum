using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.Models
{
    public class Receipt
    {
        [Key]
        public int IdReceipt { get; set; }


        [Required(ErrorMessage = "Datum je obavezan!")]
        [Display(Name = "Datum")]
        [DataType(DataType.Date)]
        public DateTime DateReceipt { get; set; }


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


        [Required(ErrorMessage = "Iznos na žiro-račun je obavezan!")]
        [Display(Name = "Na žiro-račun")]
        [DataType(DataType.Currency)]
        public decimal AmountTransferAccount { get; set; }


        [Required(ErrorMessage = "Iznos u naravi je obavezan!")]
        [Display(Name = "U naravi")]
        [DataType(DataType.Currency)]
        public decimal AmountNonCashBenefit { get; set; }


        [Required(ErrorMessage = "Iznos PDV-a je obavezan!")]
        [Display(Name = "Iznos PDV-a")]
        [DataType(DataType.Currency)]
        public decimal ValueAddedTax { get; set; }


        [Display(Name = "Ukupno")]
        [DataType(DataType.Currency)]
        public decimal Totaled { get; set; }
    }
}