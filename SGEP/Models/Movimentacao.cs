using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SGEP.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public int MaterialId { get; set; }
        public int Quantidade { get; set; }
        public int Origem { get; set; }
        public int Destino { get; set; }
        public string Tipo { get; set; }
    }
}
