using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGEP.Data;
using SGEP.Models;
using Newtonsoft.Json.Linq;

namespace SGEP.Controllers
{
    ///<summary>
    ///Este controller possuem métodos de debug e deve permanecer comentado em ambiente produção.
    ///</summary>
    public class DebugController : Controller
    {
        // private readonly ApplicationDbContext _context;
        // private readonly UserManager<SGEPUser> _userManager;
        // private readonly IHostingEnvironment _env;
        // public DebugController(ApplicationDbContext context, UserManager<SGEPUser> userManager, IHostingEnvironment env) 
        // { 
        //     _context = context;
        //     _userManager = userManager;
        //     _env = env;
        // }
        
        // public JsonResult Users()
        // {
        //     return Json(new {_userManager.Users, count = _userManager.Users.Count()});
        // }

        // [Route("Debug/Roles/{email}")]
        // public async Task<JsonResult> Roles(string email)
        // {
        //     return Json(await _userManager.GetRolesAsync(await _userManager.FindByEmailAsync(email)));
        // }
        
        // public IActionResult SubtractMP(string t0, string t)
        // {
        //     return Json((MonthPeriod) t - (MonthPeriod) t0);
        // }
        
        // [Route("Debug/DateTimeToMP/{date}")]
        // public IActionResult DateTimeToMP(DateTime? date)
        // {
        //     if (date.HasValue)
        //         return Json((MonthPeriod) date);
        //     return Json("Null");
        // }

        // public async Task<string> FileContentTest(string path)
        // {
        //     return await System.IO.File.ReadAllTextAsync(_env.ContentRootPath + path);
        // }

        // public async Task<IActionResult> JsonCreationTest()
        // {
        //     var jsonString = await System.IO.File.ReadAllTextAsync(_env.ContentRootPath + "/Seed/users.json");
        //     var usersJson = JArray.Parse(jsonString);
        //     List<SGEPUser> list = new List<SGEPUser>();
        //     foreach (JObject userJson in usersJson)
        //     {
        //         SGEPUser user = new SGEPUser()
        //         {
        //             Nome = userJson["nome"].Value<string>(),
        //             Email = userJson["email"].Value<string>(),
        //             PhoneNumber = userJson["telefone"].Value<string>(),
        //             Ativo = true,
        //         };
        //         list.Add(user);
        //         // if(user.Email != null && (await _userManager.FindByEmailAsync(user.Email)) != null)
        //         //     continue;

        //         // string role = userJson["tipo"].Value<string>();
        //         // if (role != "Almoxarife" && role != "Gerente")
        //         //     throw new InvalidDataException("Campo \"tipo\" no arquivo \"users.json\" deve ser igual a \"Almoxarife\" ou \"Gerente\"");
        //         // var result = await _userManager.CreateAsync(user);
        //         // if (!result.Succeeded)
        //         // {
        //         //     continue;
        //         // }
        //         // result = await _userManager.AddToRoleAsync(user, role);
        //         // if (!result.Succeeded)
        //         // {
        //         //     await _userManager.DeleteAsync(user);
        //         // }
        //     }
        //     return Json(list);
        // }

        // public JsonResult Almoxarifados() => Json(_context.Almoxarifado);

        // public IActionResult Page()
        // {
        //     return View();
        // }
    }
}