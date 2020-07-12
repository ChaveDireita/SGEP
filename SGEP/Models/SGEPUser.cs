using Microsoft.AspNetCore.Identity;
using SGEP.Models.Constants;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace SGEP.Models
{
    public class SGEPUser : IdentityUser
    {
        [PersonalData]
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
    }
    ///<summary>
    ///Usado para exibição e validação de dados referentes aos usuários do sistema.
    ///</summary>
    public class UserForm
    {
        public string Id { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Nome { get; set; }
        // [DataType(DataType.PhoneNumber)]
        [Phone]
        public string Telefone { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        [EmailAddress(ErrorMessage = "Esse e-mail está inválido.")]
        public string Email { get; set; }
        public bool Ativo { get; set; }
        [Required(ErrorMessage = CommonMessages.REQUIRED_FIELD)]
        public string Role { get; set; }
        public static async Task<UserForm> FromIdentity(SGEPUser user, UserManager<SGEPUser> userManager) => new UserForm {
            Id = user.Id,
            Ativo = user.Ativo,
            Email = user.Email,
            Nome = user.Nome,
            Telefone = user.PhoneNumber,
            Role = (await userManager.GetRolesAsync(user))[0]
        };
        
    }
}
