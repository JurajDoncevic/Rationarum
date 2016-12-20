using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Rationarum_v3.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }
    }

    public class ManageUserViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno polje")]
        [Display(Name = "Korisničko ime")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mail adresa je obavezno polje")]
        [EmailAddress(ErrorMessage = "Neispravan oblik e-mail adrese")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ime udruge je obavezno")]
        [Display(Name = "Ime udruge")]
        public string AssociationName { get; set; }

        [Required(ErrorMessage = "Adresa udruge je potrebna zbog generiranja godišnjih izvješća")]
        [Display(Name = "Adresa udruge")]
        public string Adress { get; set; }


        [Required(ErrorMessage = "OIB udruge je potreban zbog generiranja godišnjih izvješća")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "OIB mora imati 13 znamenki")]
        [Display(Name = "OIB udruge")]
        public string OIB { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Trenutna lozinka")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Lozinka mora imati barem {2} znakova.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova lozinka")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrdite novu lozinku")]
        [Compare("NewPassword", ErrorMessage = "Nova lozinka i potvrđena lozinka se ne podudaraju")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Korisničko ime")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [Display(Name = "Zapamti me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Korisničko ime je obavezno polje")]
        [Display(Name = "Korisničko ime")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mail adresa je obavezno polje")]
        [EmailAddress(ErrorMessage = "Neispravan oblik e-mail adrese")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ime udruge je obavezno")]
        [Display(Name = "Ime udruge")]
        public string AssociationName { get; set; }

        [Required(ErrorMessage = "Adresa udruge je potrebna zbog generiranja godišnjih izvješća")]
        [Display(Name = "Adresa udruge")]
        public string Adress { get; set; }


        [Required(ErrorMessage = "OIB udruge je potreban zbog generiranja godišnjih izvješća")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "OIB mora imati 13 znamenki")]
        [Display(Name = "OIB udruge")]
        public string OIB { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Lozinka mora imati barem {2} karaktera.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Lozinka")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Potvrdna lozinka")]
        [Compare("Password", ErrorMessage = "Lozinka nije ista kao i potvrdna lozinka.")]
        public string ConfirmPassword { get; set; }
    }

    public class UserManagementViewModel
    {
        public string Id;

        [Required(ErrorMessage = "Korisničko ime je obavezno polje")]
        [Display(Name = "Korisničko ime")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "E-mail adresa je obavezno polje")]
        [EmailAddress(ErrorMessage = "Neispravan oblik e-mail adrese")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Ime udruge je obavezno")]
        [Display(Name = "Ime udruge")]
        public string AssociationName { get; set; }

        [Required(ErrorMessage = "Adresa udruge je potrebna zbog generiranja godišnjih izvješća")]
        [Display(Name = "Adresa udruge")]
        public string Adress { get; set; }


        [Required(ErrorMessage = "OIB udruge je potreban zbog generiranja godišnjih izvješća")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "OIB mora imati 13 znamenki")]
        [Display(Name = "OIB udruge")]
        public string OIB { get; set; }

        [Required]
        [Display(Name = "Role")]
        public IdentityRole Role { get; set; }


    }
}
