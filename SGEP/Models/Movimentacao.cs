using System;
using System.ComponentModel.DataAnnotations;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    public class Movimentacao
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public DateTime Data { get; set; }
        [Display(Name = "Material")]
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public int MaterialId { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public decimal Quantidade { get; set; }
        [Display(Name = "Origem")]
        public int OrigemId { get; set; }
        [Display(Name = "Destino")]
        public int DestinoId { get; set; }
        public string Tipo { get; set; }
        [DataType(DataType.Currency)]
        public decimal Preco { get; set; }
        public virtual SGEPUser Almoxarife { get; set; }
        public int? SolicitanteId { get; set; }
        public virtual Funcionario Solicitante { get; set; }
    }
}
