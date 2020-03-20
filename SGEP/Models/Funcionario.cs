using System.ComponentModel.DataAnnotations;

namespace SGEP.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public bool Demitido { get; set; } = false;
        [RegularExpression("[0-9]{4}", ErrorMessage = "A matrícula deve possuir 4 números")]
        public string Matricula { get; set; }
    }
}