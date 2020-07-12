using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    ///<summary>
    ///É usado para validar os dados inseridos no formulário em Views/Histograma/Partial/_Form.cshtml
    ///</summary>
    public class HistogramaViewModel
    {
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Inicio { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        [Remote(action: "ValidateDate", controller: "Histograma", AdditionalFields = "Inicio")]
        public string Fim { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public int Material { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public int? Almoxarifado { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Tipo { get; set; }
    }
}