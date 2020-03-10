using System.Collections.Generic;

namespace SGEP.Models
{
    public class Almoxarifado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Projeto { get; set; }
        public int AlmoxarifadosxMateriaisId {get; set; }
        public virtual List<AlmoxarifadosxMateriais> AlmoxarifadosxMateriais { get; set; }
    }
}