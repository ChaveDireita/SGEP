using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    public class Funcionario
    {
        private const string nomeRegex = "[A-zàãáâèéêìíîòõóôùúûÁÀÃÂÈÉÊÌÍÎÓÒÕÔÙÚÛ][A-z àãáâèéêìíîòõóôùúûÁÀÃÂÈÉÊÌÍÎÓÒÕÔÙÚÛ]+";
        public int Id { get; set; }
        [RegularExpression(nomeRegex, ErrorMessage = "O nome deve ser composto por apenas por letras.")]
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Nome { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]

        public string Cargo { get; set; }
        public bool Ativo { get; set; } = true;
        [Display(Name = "Matrícula")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "A matrícula deve possuir 4 números")]
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        [Remote(action: "VerificarMatricula", controller: "Funcionario", AdditionalFields = "Id")]
        public string Matricula { get; set; }
    }
}