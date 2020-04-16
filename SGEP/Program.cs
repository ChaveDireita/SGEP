using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SGEP.Models;

namespace SGEP
{
    public class Program
    {
        public static async Task Main (string[] args)
        {
            var host = CreateWebHostBuilder (args).Build ();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var userManager = serviceProvider.GetRequiredService<UserManager<SGEPUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                if (!(await roleManager.RoleExistsAsync("Almoxarixe")))
                    await roleManager.CreateAsync(new IdentityRole("Almoxarife"));
                if (!(await roleManager.RoleExistsAsync("Gerente")))
                    await roleManager.CreateAsync(new IdentityRole("Gerente"));

                if ((await userManager.FindByEmailAsync("admin@admin")) == null)
                {
                    SGEPUser user = new SGEPUser {Nome = "Admin", Email = "admin@admin", PhoneNumber = "11112222", UserName = "admin"};
                    await userManager.CreateAsync(user, "12345678");
                    if (await userManager.FindByEmailAsync("admin@admin") == null)
                        throw new NullReferenceException("User not created");
                    await userManager.AddToRolesAsync(user, new string[] { "Almoxarife", "Gerente" });
                }
            }
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
                .UseStartup<Startup> ();
    }
}
