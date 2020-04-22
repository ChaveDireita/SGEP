using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGEP.Models
{
    public class Material
    {
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        public string Descricao { get; set; }
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }
        public int Categoria { get; set; }
        public string Showid { get; set; }
    }
}