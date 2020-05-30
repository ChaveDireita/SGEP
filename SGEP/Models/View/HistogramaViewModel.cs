using System;
using System.ComponentModel.DataAnnotations;

using SGEP.Models.Constants;

namespace SGEP.Models
{
    public class HistogramaViewModel
    {
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public string Inicio { get; set; }
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public string Fim { get; set; }
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public int Material { get; set; }
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public int? Almoxarifado { get; set; }
        [Required(ErrorMessage = CommomErrorMessages.REQUIRED_FIELD)]
        public string Tipo { get; set; }
    }
}