using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SGEP.Models;

namespace SGEP
{
    public class RolesSeeder
    {
        private readonly UserManager<SGEPUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RolesSeeder(UserManager<SGEPUser> userManager, RoleManager<IdentityRole> roleManager)
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
        }
    }
}