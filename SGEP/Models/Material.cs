using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SGEP.Models
{
    public class Material
    {
        [Display(Name ="ID")]
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }
        public int Categoria { get; set; }
        [Display(Name ="Unidade")]
        public int IdUnidade { get; set; }
        [Display(Name ="Preço/Unidade")]
        public string Precounidade { get; set; }
        public string Showid { get; set; }
    }
}