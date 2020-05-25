using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace SGEP.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public bool Ativo { get; set; } = true;
        [Display(Name = "Matrícula")]
        [RegularExpression("[0-9]{4}", ErrorMessage = "A matrícula deve possuir 4 números")]
        [Required]
        [Remote(action: "VerificarMatricula", controller: "Funcionario", AdditionalFields = "Id,Ativo")]
        public string Matricula { get; set; }
    }
}