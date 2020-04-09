using Microsoft.AspNetCore.Identity;

namespace SGEP.Models
{
    public class SGEPUser : IdentityUser
    {
        [PersonalData]
        public string CPF { get; set; }//Tirar
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

        public static UserForm FromIdentity(SGEPUser user) => new UserForm {
            Id = user.Id,
            Ativo = user.Ativo,
            Email = user.Email,
            Nome = user.Nome,
            Telefone = user.PhoneNumber
        };
    }
}
