using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SGEP.Data;
using SGEP.Models;

namespace SGEP.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly UserManager<SGEPUser> _userManager;
        private readonly SignInManager<SGEPUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public UserController(UserManager<SGEPUser> userManager, SignInManager<SGEPUser> signInManager, ApplicationDbContext context) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<JsonResult> Get(string id)
        {
            SGEPUser user = await _userManager.FindByIdAsync(id);
            return Json(UserForm.FromIdentity(user));
        }
        public JsonResult List (string id, string email, string nome, string telefone, int? itensPorPagina, int? pagina)
        {
            IEnumerable<SGEPUser> result = _userManager.Users.ToList();
            /*if (id != null && id.Trim () != "")
                result = result.Where (f => f.Id.ToString ().Contains (id));
            if (nome != null && nome?.Trim () != "")
                result = result.Where (f => f.Nome.Contains (nome));
            if (cargo != null && cargo?.Trim () != "")
                result = result.Where (f => f...Contains (cargo));
            int inicio = (itensPorPagina ?? 10) * ((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count () - inicio);
            result = result.ToList ().GetRange (inicio, qtd);*/

            return Json (new { size = _userManager.Users.Count (), entities = result });
        }
        public async Task<IActionResult> Create([Bind("Nome,Email,Telefone")] UserForm user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new SGEPUser { Email = user.Email, Nome = user.Nome, UserName = user.Email, PhoneNumber = user.Telefone }, "12345678");
                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest();
        }
        public async Task<IActionResult> Edit([Bind("Id,Nome,Email,Telefone")] UserForm user)
        {
            if (ModelState.IsValid)
            {
                SGEPUser savedInfo = await _userManager.FindByIdAsync(user.Id);
                if (savedInfo == null)
                    return BadRequest("User does not exist");
                savedInfo.Nome = user.Nome;
                savedInfo.Email = user.Email;
                savedInfo.PhoneNumber = user.Telefone;

                var result = await _userManager.UpdateAsync(savedInfo);
                if (result.Succeeded)
                    return Ok();
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest();
        }
        public async Task<IActionResult> ChangePassword(string id, string old, string pass, string confirm)
        {
            SGEPUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest("User does not exist");
            if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, old) == PasswordVerificationResult.Failed)
                return BadRequest("Provided old password is wrong");
            if (pass != confirm)
                return BadRequest("The new password didn't match with confirm");
            await _signInManager.SignOutAsync();
            await _userManager.ChangePasswordAsync(user, old, pass);
            return Ok("Success");
        }

        public async Task<IActionResult> Profile()
        {
            if (_signInManager.IsSignedIn(User))
            {
                SGEPUser user = await _userManager.GetUserAsync(User);
                if (user == null)
                    return BadRequest("User does not exist");
                return View(user);
            }
            return BadRequest("User is not signed in");
        }
    }
}