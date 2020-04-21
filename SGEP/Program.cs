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
            var host = CreateWebHostBuilder (args).Build();//.Run();
            if (args.Length > 0)
            {
                string mode = args[0];
                if (mode == "--add-user" || mode == "-AU")
                {
                    using(var scope = host.Services.CreateScope())
                        await CreateUser(scope.ServiceProvider.GetService<UserManager<SGEPUser>>());
                }
                else if(mode == "--help" || mode == "-h")
                {
                    Help();
                } 
                else {
                    Console.WriteLine($"Unknown argument \"{mode}\" provided\n\n");
                    Help();
                }
            }
            else
            {
                host.Run();
            }
        }

        private static void Help()
        {
            Console.WriteLine("Usage: SGEP [--add-user | --help]\n");
            Console.WriteLine("Arguments:");
            Console.WriteLine("     --add-user adds a new user");
            Console.WriteLine("     --help     show usage and command list");
        }

        private static async Task CreateUser(UserManager<SGEPUser> manager)
        {
            SGEPUser user = new SGEPUser();
            Console.WriteLine("Creating user: \n");
            Console.WriteLine("Nome: ");
            user.Nome = Console.ReadLine();
            Console.WriteLine("E-mail: ");
            user.Email = Console.ReadLine();
            Console.WriteLine("Telefone: ");
            user.PhoneNumber = Console.ReadLine();
            user.UserName = user.Email;
            Console.WriteLine("Role: ");
            Console.WriteLine("1 - Almoxarife ");
            Console.WriteLine("2 - Gerente ");
            string role = Console.ReadLine() == "1" ? "Almoxarife" : "Gerente";
            

            var result = await manager.CreateAsync(user, "12345678");
            if (result.Succeeded)
            {
                await manager.AddToRoleAsync(user, role);
                Console.WriteLine("User successfully created!");
                Console.WriteLine("Login with these credentials: ");
                Console.WriteLine("E-mail: " + user.Email);
                Console.WriteLine("Password: 12345678");
                return;
            }
            Console.WriteLine("There was an error on creating user: ");
            foreach (var error in result.Errors)
            {
                Console.WriteLine(error.Code + ": " + error.Description);
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
                .UseStartup<Startup> ();
    }
}
