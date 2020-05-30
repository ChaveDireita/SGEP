using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    public class Material
    {
        [Display(Name ="ID")]
        public int Id { get; set; }
        [DataType(DataType.MultilineText)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public string Descricao { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Preço")]
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public decimal Preco { get; set; }
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public int Categoria { get; set; }
        [Display(Name ="Unidade")]
        public int IdUnidade { get; set; }
        [Display(Name ="Preço/Unidade")]
        public string Precounidade { get; set; }
        public string Showid { get; set; }
    }
}