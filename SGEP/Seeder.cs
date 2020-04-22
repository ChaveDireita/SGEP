using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SGEP.Models;

namespace SGEP
{
    public class UserAndRolesSeeder
    {
        private readonly UserManager<SGEPUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserAndRolesSeeder(UserManager<SGEPUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            
            if (!(await _roleManager.RoleExistsAsync("Almoxarixe")))
                    await _roleManager.CreateAsync(new IdentityRole("Almoxarife"));
            if (!(await _roleManager.RoleExistsAsync("Gerente")))
                await _roleManager.CreateAsync(new IdentityRole("Gerente"));

            if ((await _userManager.FindByEmailAsync("admin@admin")) == null)
            {
                SGEPUser user = new SGEPUser {Nome = "Admin", Email = "admin@admin", PhoneNumber = "11112222", UserName = "admin@admin"};
                var result = await _userManager.CreateAsync(user, "12345678");
                if (!result.Succeeded)
                    throw new NullReferenceException("User not created");
                await _userManager.AddToRolesAsync(user, new string[] { "Almoxarife", "Gerente" });
            }
        }
    }
}