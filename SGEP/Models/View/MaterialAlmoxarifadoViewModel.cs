using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGEP.Models.View
{
    public class MaterialAlmoxarifadoViewModel
    {
        public int Id { get; set; }
        public string Showid { get; set; }
        public int Categoria { get; set; }
        public string Descricao { get; set; }
        public string QuantidadeTotal { get; set; }
        public string Unidade { get; set; }
        public  decimal Preco { get; set; }
        public string PrecoUnidade { get; set; }
        public int AlmoxarifadoId { get; set; }
        public int IdUnidade { get; set; }
    }
}
