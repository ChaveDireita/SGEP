using System.ComponentModel.DataAnnotations;

namespace SGEP.Models
{
    ///<summary>
    ///É usado para validar os dados inseridos no formulário em Views/User/_ChangePassword.cshtml
    ///</summary>
    public class ChangePasswordViewModel
    {
        public string Id { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Old { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string New { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [Compare("New", ErrorMessage = "Os campos \"Nova senha\" e \"Confirme a nova senha\" estão diferentes.")]
        public string Confirm { get; set; }
    }
}