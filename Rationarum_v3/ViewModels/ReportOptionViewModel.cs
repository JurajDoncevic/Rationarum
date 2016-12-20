using Rationarum_v3.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Rationarum_v3.ViewModels
{
    public class ReportOptionViewModel
    {
        [Required]
        [Display(Name = "Vrsta izvješća")]
        public string ReportType { get; set; }
        
        [Required]
        [Display(Name = "Godina")]
        public string ReportYear { get; set; }
    }
}