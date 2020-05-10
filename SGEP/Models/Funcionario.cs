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
        [RegularExpression("[0-9]{4}", ErrorMessage = "A matrícula deve possuir 4 números")]
        [Remote(action: "VerificarMatricula", controller: "Funcionario")]
        public string Matricula { get; set; }
    }
}