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
        public Almoxarifado Origem { get; set; }
        public Almoxarifado Destino { get; set; }
        public string Tipo { get; set; }
    }
}
