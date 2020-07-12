using System.ComponentModel.DataAnnotations;

namespace SGEP.Models
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress(ErrorMessage = "Esse e-mail está inválido.")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Email { get; set; }
    }
}