using System.Collections.Generic;

namespace SGEP.Models
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cargo { get; set; }
        public List<Projeto> ProjetosAssociados { get; set; }
    }
}