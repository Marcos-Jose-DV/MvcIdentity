using System.ComponentModel.DataAnnotations;

namespace MvcWebIdentity.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O Emial é Obrigatório")]
        [EmailAddress(ErrorMessage = "Email Inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Senha é Obrigatoria")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool RememberMe { get; set; }
    }
}
