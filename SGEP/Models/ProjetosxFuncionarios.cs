namespace SGEP.Models
{
    ///<summary>
    ///Representa a relação entre funcionários e projetos. É uma relação n para n.
    ///</summary>
    public class ProjetosxFuncionarios
    {
        public int Id { get; set; }
        public int IdFuncionario { get; set; }
        public int IdProjeto { get; set; }
    }
}
