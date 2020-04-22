using System.Collections.Generic;

namespace SGEP.Models
{
    public class AlmoxarifadosxMateriais
    {
        public int AlmoxarifadoId { get; set; }
        public int MaterialId { get; set; }
        public virtual List<Material> Material { get; set; }
        public int Quantidade { get; set; }
    }
}