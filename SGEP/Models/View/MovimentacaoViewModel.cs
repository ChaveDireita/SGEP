using System;
using System.ComponentModel.DataAnnotations;

namespace SGEP.Models.View
{
    public class MovimentacaoViewModel
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }
        [Display(Name = "ID do Material")]
        public int MaterialId { get; set; }
        [DataType(DataType.Currency)]
        public string Quantidade { get; set; }
        [Display(Name = "Origem")]
        public int OrigemId { get; set; }
        [Display(Name = "Destino")]
        public int DestinoId { get; set; }
        public string Tipo { get; set; }
        [DataType(DataType.Currency)]
        public string Preco { get; set; }
        public string Solicitante { get; set; }
        public SGEPUser Almoxarife { get; set; }
    }
}
