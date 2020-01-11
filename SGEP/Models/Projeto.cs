using System;
using System.Collections.Generic;

namespace SGEP.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime Inicio { get; set; }
        public DateTime Fim { get; set; }
        public List<Funcionario> Funcionarios { get; set; }
        //public int AlmoxarifadoId { get; set; }
    }
}