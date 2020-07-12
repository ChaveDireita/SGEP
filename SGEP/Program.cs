using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
        // Precisa eventualmente para scaffold
        // public static void Main (string[] args)
        // {
        //      CreateWebHostBuilder (args).Build ().Run();
        // }
        
        public static async Task Main (string[] args)
        {
            var host = CreateWebHostBuilder (args)
                    //    .UseKestrel()
                    //    .UseContentRoot(Directory.GetCurrentDirectory())
                    //    .UseIISIntegration()
                    //    .UseStartup<Startup>()
                       .Build();//.Run();
            if (args.Length > 0)
            {
                string option = args[0];
                if (option == "--add-user" || option == "-AU")
                {
                    using(var scope = host.Services.CreateScope())
                        await CreateUser(scope.ServiceProvider.GetRequiredService<UserManager<SGEPUser>>(), 
                                         scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>());
                }
                else if(option == "--help" || option == "-h")
                {
                    Help();
                } 
                else {
                    Console.WriteLine($"Argumento desconhecido, \"{option}\", inserido.\n\n");
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
            Console.WriteLine("Uso: SGEP [--add-user | --help]\n");
            Console.WriteLine("Argumentos:");
            Console.WriteLine("     --add-user adiciona um novo usuário");
            Console.WriteLine("     --help     exibe o uso e a lista de argumentos");
        }

        private static async Task CheckForRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!(await roleManager.RoleExistsAsync("Almoxarife")))
                    await roleManager.CreateAsync(new IdentityRole("Almoxarife"));
            if (!(await roleManager.RoleExistsAsync("Gerente")))
                await roleManager.CreateAsync(new IdentityRole("Gerente"));
        }
        private static async Task CreateUser(UserManager<SGEPUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            SGEPUser user = new SGEPUser();
            Console.WriteLine("Creating user: \n");
            Console.WriteLine("Nome: ");
            string _nome = Console.ReadLine();
            while(string.IsNullOrWhiteSpace (_nome))
            {
                Console.WriteLine("Nome inválido.");
                _nome = Console.ReadLine();
            }
            user.Nome = _nome; 
            Console.WriteLine("E-mail: ");
            string _email = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(_email) || !Regex.Match(_email, "[A-z0-9.\\-_]+@[a-z]+([.][a-z]+){1,2}").Success)
            {
                Console.WriteLine("E-mail inválido.");
                _nome = Console.ReadLine();
            }
            user.Email = _email;
            Console.WriteLine("Telefone: ");
            string _telefone = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(_telefone) || !Regex.Match(_telefone, "[0-9]{8,9}").Success)
            {
                Console.WriteLine("Telefone inválido.");
                _telefone = Console.ReadLine();
            }
            user.PhoneNumber = _telefone;
            
            user.UserName = user.Email;
            
            Console.WriteLine("Tipo: ");
            Console.WriteLine("1 - Almoxarife ");
            Console.WriteLine("2 - Gerente ");
            string _role = Console.ReadLine();
            while (string.IsNullOrWhiteSpace(_role) || _role != "1" && _role != "2")
            {
                Console.WriteLine("Insira 1 ou 2.");
                _role = Console.ReadLine();
            }
            _role = _role == "1" ? "Almoxarife" : "Gerente";
            var userResult = await userManager.CreateAsync(user, "12345678");
            if (userResult.Succeeded)
            {
                await CheckForRoles(roleManager);
                var roleResult = await userManager.AddToRoleAsync(user, _role);
                if (roleResult.Succeeded)
                {
                    Console.Clear();
                    Console.WriteLine("Usuário criado com sucesso!");
                    Console.WriteLine("Entre com as seguintes credenciais: ");
                    Console.WriteLine("E-mail: " + user.Email);
                    Console.WriteLine("Senha: 12345678");
                } else {
                    await userManager.DeleteAsync(user);
                    Console.WriteLine("Ocorreu um erro ao criar um usuário: ");
                    foreach (var error in roleResult.Errors)
                    {
                        Console.WriteLine(error.Code + ": " + error.Description);
                    }
                }
                return;
            }
            Console.WriteLine("Ocorreu um erro ao criar um usuário: ");
            foreach (var error in userResult.Errors)
            {
                Console.WriteLine(error.Code + ": " + error.Description);
            }
        }
        public static IWebHostBuilder CreateWebHostBuilder (string[] args) =>
            WebHost.CreateDefaultBuilder (args)
                .UseStartup<Startup> ();
    }
}
