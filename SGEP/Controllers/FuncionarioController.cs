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
    [Authorize(Roles = "Almoxarife,Gerente")]
    public class FuncionarioController : Controller
    {
        private readonly ApplicationDbContext _context;
        public FuncionarioController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();

        [HttpGet("/Funcionario/Get/{id}")]
        public async Task<JsonResult> Get (int id) => Json(await _context.Funcionario.FindAsync(id));

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
        public async Task<IActionResult> Create([Bind("Id,Nome,Cargo,Matricula")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return Ok("Funcionário adicionado com sucesso!");
            }
            return BadRequest("Ocorreu um erro ao adicionar o funcionário.");
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cargo,Ativo,Matricula")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
                return NotFound("O funcionário não existe.");
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
                        return NotFound("O funcionário não existe.");
                    else
                        throw;
                }
                return Ok("As alterações foram salvas com sucesso.");
            }
            return BadRequest("Ocorreu um erro ao salvar as alterações.");
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = await _context.Funcionario.FirstAsync(f => f.Id == id);
            if (funcionario == null)
                return NotFound();
            funcionario.Ativo = false;
            _context.Update(funcionario);
            _context.RemoveRange(_context.ProjetosxFuncionarios.Where(pf => pf.IdFuncionario == funcionario.Id));
            await _context.SaveChangesAsync();
            return Ok();
        }
        public async Task<JsonResult> ProjetosAssociados(int? id)
        {
            IEnumerable<int> Ids = (await _context.ProjetosxFuncionarios.ToListAsync())
                                                                        .Where(f => f.IdFuncionario == id)
                                                                        .Select(p => p.IdProjeto);
            List<Projeto> projetos = await _context.Projeto.Where(p => Ids.Contains(p.Id))
                                                           .ToListAsync();
            return Json(projetos);
        }
        public IActionResult WillFail() => Ok("Dummy Response");
        private bool FuncionarioExists(int id) =>  _context.Funcionario.Any(e => e.Id == id);
    }
}