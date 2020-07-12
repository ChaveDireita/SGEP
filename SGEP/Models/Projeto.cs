using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    public class Projeto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Nome { get; set; }
        [Display(Name = "Data de Início")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public DateTime Inicio { get; set; }
        [Display(Name = "Data de Término")]
        [DataType(DataType.Date)]
        [Remote(action: "ValidateDate", controller: "Projeto", AdditionalFields = "Inicio")]
        public DateTime? Fim { get; set; }
        public int AlmoxarifadoId { get; set; }
        public virtual Almoxarifado Almoxarifado { get; set; }
        public bool Finalizado { get => Fim != null; }

        public object ToBeShow()
        {
            return new {
                Id,
                Nome,
                Almoxarifado,
                Inicio = Inicio.ToShortDateString(),
                Fim = Fim == null ? "--" : Fim.GetValueOrDefault().ToShortDateString()
            };
        }
    }
}