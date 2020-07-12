using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    ///<summary>
    ///Representa as unidades de medida dos materiais.
    ///</summary>
    public class Unidade
    {
        public int Id { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Nome { get; set; }
        public string Abreviacao { get; set; }
    }
}
