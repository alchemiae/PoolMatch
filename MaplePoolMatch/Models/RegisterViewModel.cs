using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace MaplePoolMatch.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Vyplňte emailovou adresu")]
        [EmailAddress(ErrorMessage = "Neplatná emailová adresa")]
        [Display(Name = "E-mail")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Vyplňte heslo")]
        [StringLength(100, ErrorMessage = "{0} musí mít délku alespoň {2} a nejvíc {1} znaků.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Znovu zadejte heslo")]
        [Display(Name = "Potvrzení hesla")]
        [Compare(nameof(Password), ErrorMessage = "Zadaná hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; } = "";
    }
}
