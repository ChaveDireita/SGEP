using System.Collections.Generic;

namespace SGEP.Models
{
    ///<summary>
    ///Representa a relação entre almoxarifados(estoques) e materiais no banco. A relação é n para n.
    ///</summary>
    public class AlmoxarifadosxMateriais
    {
        public int AlmoxarifadoId { get; set; }
        public int MaterialId { get; set; }
        public virtual List<Material> Material { get; set; }
        public decimal Quantidade { get; set; }
    }
}