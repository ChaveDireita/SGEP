using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace SGEP.Models
{
    public class SGEPUser : IdentityUser
    {
        [PersonalData]
        public string Nome { get; set; }
        public bool Ativo { get; set; } = true;
    }

    public class UserForm
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }
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
