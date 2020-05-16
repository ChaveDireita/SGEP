using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGEP.Models.View
{
    public class MaterialAlmoxarifadoViewModel
    {
        public int MaterialId { get; set; }
        public string MaterialShowId { get; set; }
        public int Categoria { get; set; }
        public string DescMaterial { get; set; }
        public decimal QuantidadeTotal { get; set; }
        public string Unidade { get; set; }
        public  decimal Preco { get; set; }
        public string PrecoUnidade { get; set; }
        public int AlmoxarifadoId { get; set; }
    }
}
