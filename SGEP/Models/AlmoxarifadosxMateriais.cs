namespace SGEP.Models
{
    public class AlmoxarifadosxMateriais
    {
        public int AlmoxarifadoId { get; set; }
        public virtual Almoxarifado Almoxarifado { get; set; }
        public int MaterialId { get; set; }
        public virtual Material Material { get; set; }
        public decimal Quantidade { get; set; }
    }
}