using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGEP.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [Display(Name = "Início")]
        [DataType(DataType.Date)]
        public DateTime Inicio { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Fim { get; set; }
        public int AlmoxarifadoId { get; set; }
        public virtual Almoxarifado Almoxarifado { get; set; }
        public bool Finalizado { get => Fim != null; }
    }
}