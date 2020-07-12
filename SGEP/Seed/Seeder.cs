using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json.Linq;
using SGEP.Models;

namespace SGEP
{
    ///<summary>
    ///Popula o banco de dados.
    ///</summary>
    public class Seeder
    {
        private readonly UserManager<SGEPUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly string _contentRootDirectory;
        public Seeder(UserManager<SGEPUser> userManager, RoleManager<IdentityRole> roleManager, IHostingEnvironment env)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contentRootDirectory = env.ContentRootPath;
        }
        ///<summary>
        ///Popula o banco com os "Roles" e com os usuários descritos em Seed/users.json.
        ///</summary>
        public async Task Seed()
        {
            if (!(await _roleManager.RoleExistsAsync("Almoxarixe")))
                    await _roleManager.CreateAsync(new IdentityRole("Almoxarife"));
            if (!(await _roleManager.RoleExistsAsync("Gerente")))
                await _roleManager.CreateAsync(new IdentityRole("Gerente"));
            
            var jsonString = await File.ReadAllTextAsync(_contentRootDirectory + "/Seed/users.json");
            var usersJson = JArray.Parse(jsonString);
            
            foreach (JObject userJson in usersJson)
            {
                SGEPUser user = new SGEPUser()
                {
                    Nome = userJson["nome"].Value<string>(),
                    Email = userJson["email"].Value<string>(),
                    Ativo = true,
                };
                user.UserName = user.Email;
                if(user.Email != null && (await _userManager.FindByEmailAsync(user.Email)) != null)
                    continue;

                string role = userJson["tipo"].Value<string>();
                if (role != "Almoxarife" && role != "Gerente")
                    throw new InvalidDataException("Campo \"tipo\" no arquivo \"users.json\" deve ser igual a \"Almoxarife\" ou \"Gerente\"");
                var result = await _userManager.CreateAsync(user, "12345678");
                if (!result.Succeeded)
                {
                    continue;
                }
                result = await _userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    await _userManager.DeleteAsync(user);
                }
            }
        }
    }
}