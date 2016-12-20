using Rationarum_v3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.ViewModels
{
    public abstract class ExpenditureReceiptViewModel
    {
        public int Id { get; set; }


        [Required(ErrorMessage = "Datum je obavezan!")]
        [Display(Name = "Datum")]
        [RegularExpression(@"^([1-9]|[12][0-9]|3[01])[.]([1-9]|1[012])[.](19|20)\d\d\.$", ErrorMessage = "Datum nije u obliku d.m.yyyy.")]
        public string Date { get; set; }

        [Required(ErrorMessage = "Broj temeljnice je obavezan!")]
        [StringLength(128)]
        [Column(TypeName = "varchar")]
        [Display(Name = "Temeljnica/Opis dokumenta")]
        public string JournalEntryNum { get; set; }


        [Required]
        public string ApplicationUserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}