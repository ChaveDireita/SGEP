using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGEP.Data;
using SGEP.Models;

namespace SGEP.Controllers
{
    public class FuncionarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FuncionarioController(ApplicationDbContext context) => _context = context;
        [Authorize(Roles = "Almoxarife,Gerente")]
        public IActionResult Index() => View();

        [Authorize(Roles = "Almoxarife,Gerente")]
        [HttpGet("/Funcionario/Get/{id}")]
        public async Task<JsonResult> Get (int id) => Json(await _context.Funcionario.FindAsync(id));

        [Authorize(Roles = "Almoxarife,Gerente")]
        public JsonResult List(string id, string nome, string cargo, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Funcionario> result = _context.Funcionario;
            if (id != null && id.Trim() != "")
                result = result.Where(f => f.Id.ToString().Contains(id));
            if (nome != null && nome?.Trim() != "")
                result = result.Where(f => f.Nome.Contains(nome));
            if (cargo != null && cargo?.Trim() != "")
                result = result.Where(f => f.Cargo.Contains(cargo));
            int inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - inicio);
            result = result.ToList().GetRange(inicio, qtd);
            
            return Json(new {size = _context.Funcionario.Count(), entities = result});
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cargo")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cargo")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(funcionario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FuncionarioExists(funcionario.Id))
                        return NotFound();
                    else
                        throw;
                }
                return Ok();
            }
            return BadRequest();
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionario.FirstAsync(f => f.Id == id);
            if (funcionario == null)
                return NotFound();
            
            _context.Funcionario.Remove(funcionario);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Authorize(Roles = "Almoxarife,Gerente")]
        public async Task<JsonResult> ProjetosAssociados(int? id)
        {
            IEnumerable<int> Ids = (await _context.ProjetosxFuncionarios.ToListAsync())
                                                                        .Where(f => f.IdFuncionario == id)
                                                                        .Select(p => p.IdProjeto);
            List<Projeto> projetos = await _context.Projeto.Where(p => Ids.Contains(p.Id))
                                                           .ToListAsync();
            return Json(projetos);
        }
        private bool FuncionarioExists(int id) =>  _context.Funcionario.Any(e => e.Id == id);
    }
}