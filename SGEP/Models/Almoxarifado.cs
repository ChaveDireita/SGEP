using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SGEP.Models.Constants;

namespace SGEP.Models
{
    ///<summary>
    ///Representa os almoxarifados e estoques dos projetos.
    ///</summary>
    public class Almoxarifado
    {
        public int Id { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Nome { get; set; }
        public bool Projeto { get; set; }
        public bool Ativo { get; set; } = true;
        public int AlmoxarifadosxMateriaisId {get; set; }
        public virtual List<AlmoxarifadosxMateriais> AlmoxarifadosxMateriais { get; set; }
        public int? IdProjeto { get; set; }  
    }
}