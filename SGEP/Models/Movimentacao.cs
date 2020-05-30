using System;
using System.ComponentModel.DataAnnotations;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        [Display(Name = "Material")]
        public int MaterialId { get; set; }
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public int Quantidade { get; set; }
        [Display(Name = "Origem")]
        public int OrigemId { get; set; }
        [Display(Name = "Destino")]
        public int DestinoId { get; set; }
        public string Tipo { get; set; }
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }
    }
}
