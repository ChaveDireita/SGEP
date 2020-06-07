using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SGEP.Models.Constants;

namespace SGEP.Models
{
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