using System.Collections.Generic;

namespace SGEP.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Categoria { get; set; }
        public int AlmoxarifadosxMateriaisId { get; set; }
        public virtual List<AlmoxarifadosxMateriais> AlmoxarifadosxMateriais { get; set; }
    }
}