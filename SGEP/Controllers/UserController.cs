using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using SGEP.Data;
using SGEP.Models;

namespace SGEP.Controllers
{
    [Authorize(Roles="Gerente,Almoxarife")]
    public class UserController : Controller
    {
        private readonly UserManager<SGEPUser> _userManager;
        private readonly SignInManager<SGEPUser> _signInManager;
        private readonly ApplicationDbContext _context;
        private readonly IEmailSender _emailSender;
        public UserController(UserManager<SGEPUser> userManager, SignInManager<SGEPUser> signInManager, ApplicationDbContext context, IEmailSender emailSender) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _emailSender = emailSender;
        }
        [Authorize(Roles="Gerente")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles="Gerente")]
        public async Task<JsonResult> Get(string id)
        {
            SGEPUser user = await _userManager.FindByIdAsync(id);
            return Json(await UserForm.FromIdentity(user, _userManager));
        }
        [Authorize(Roles="Gerente")]
        public async Task<JsonResult> List (string email, string nome, string telefone, string tipo, int? itensPorPagina, int? pagina)
        {
            List<SGEPUser> users = _userManager.Users.ToList();
            List<UserForm> preResult = new List<UserForm>();
            foreach (var u in users)
            {
                preResult.Add(await UserForm.FromIdentity(u, _userManager));
            }
            IEnumerable<UserForm> result = preResult;
            if (email != null && email?.Trim () != "")
                result = result.Where (u => u.Email.Contains (email));
            if (nome != null && nome?.Trim () != "")
                result = result.Where (u => u.Nome.Contains (nome));
            if (telefone != null && telefone?.Trim () != "")
                result = result.Where (u => u.Telefone.Contains (telefone));
            if (tipo != null && tipo?.Trim () != "")
                result = result.Where (u => u.Role.Contains (tipo));
            int inicio = (itensPorPagina ?? 10) * ((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count () - inicio);
            int _size = result.Count();
            result = result.ToList ().GetRange (inicio, qtd);
            return Json (new { size = _size, entities = result });
        }
        [Authorize(Roles="Gerente")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Nome,Email,Telefone,Role")] UserForm user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userManager.CreateAsync(new SGEPUser { Email = user.Email, Nome = user.Nome, UserName = user.Email, PhoneNumber = user.Telefone }, "12345678");
                if (result.Succeeded)
                {
                    SGEPUser savedUser = await _userManager.FindByEmailAsync(user.Email);
                    var roleResult = await _userManager.AddToRoleAsync(savedUser, user.Role);
                    if (roleResult.Succeeded)
                        return Ok("O usuário foi criado com sucesso.");
                    else
                    {
                        await _userManager.DeleteAsync(savedUser);
                        return BadRequest(result.Errors);
                    }
                }
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest("Ocorreu um erro ao criar o usuário. Verifique a validade dos dados inseridos.");
        }
        [Authorize(Roles="Gerente")]
        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Nome,Email,Telefone,Role")] UserForm user)
        {
            if (ModelState.IsValid)
            {
                SGEPUser savedUser = await _userManager.FindByIdAsync(user.Id);
                if (savedUser == null)
                    return BadRequest("ERRO: O usuário não existe");
                savedUser.Nome = user.Nome;
                savedUser.Email = user.Email;
                savedUser.PhoneNumber = user.Telefone;
                string currentRole = (await _userManager.GetRolesAsync(savedUser))[0];
                await _userManager.RemoveFromRoleAsync(savedUser, currentRole);
                await _userManager.AddToRoleAsync(savedUser, user.Role);

                var result = await _userManager.UpdateAsync(savedUser);

                if (result.Succeeded)
                    return Ok("As alterações foram salvas com sucesso.");
                else
                    return BadRequest(result.Errors);
            }
            return BadRequest("As informações inseridas são inválidas.");
        }
        [Authorize(Roles="Gerente")]
        [HttpPost]
        public async Task<IActionResult> Toggle(string id)
        {
            SGEPUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return BadRequest("O usuário não existe.");
            user.Ativo = !user.Ativo;
            await _userManager.UpdateAsync(user);
            return Ok("O usuário foi removido com sucesso.");
        }
        public async Task<IActionResult> ChangePassword([Bind("")] ChangePasswordViewModel model)
        {
            SGEPUser user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
                return BadRequest("O usuários não existe.");
            if (_userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Old) == PasswordVerificationResult.Failed)
                return BadRequest("A senha atual inserida está incorreta.");
            if (model.New != model.Confirm)
                return BadRequest("Os campos \"Nova senha\" e \"Confirme a nova senha\" estão diferentes.");
            await _signInManager.SignOutAsync();
            await _userManager.ChangePasswordAsync(user, model.Old, model.New);
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
        [HttpPost]
        public async Task<IActionResult> SendResetPasswordEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return BadRequest("O e-mail inserido é inválido.");
                
            SGEPUser user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { code },
                    protocol: Request.Scheme);
                    
                string url = "/Account/ResetPassword/?code=" + HttpUtility.HtmlEncode(code);
                await _emailSender.SendEmailAsync(
                    email,
                    "SGEP - Recuperação de senha",
                    $"Por favor, altere sua senha <a href='{url}'>clicando aqui</a>.");
            }
            return Ok("Link enviado com sucesso. Verifique a caixa de entrada de seu e-mail.");
        }
    }
}