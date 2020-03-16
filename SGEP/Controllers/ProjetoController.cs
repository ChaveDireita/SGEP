using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SGEP.Data;
using SGEP.Models;
using System.Reflection;

namespace SGEP.Controllers
{
    [AllowAnonymous]
    public class ProjetoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProjetoController(ApplicationDbContext context) => _context = context;
        
        public IActionResult Index() => View();
        public async Task<JsonResult> List(string id, string nome, DateTime? inicio, DateTime? fim, int[] funcionarios, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Projeto> result = await _context.Projeto.Include(p => p.Almoxarifado).ToListAsync();
            if (id != null && id.Trim() != "")
                result = result.Where(p => p.Id.ToString().Contains(id));
            if (nome != null && nome.Trim() != "")
                result = result.Where(p => p.Nome.Contains(nome));
            if (inicio != null && inicio?.ToString().Trim() != "")
                result = result.Where(p => p.Inicio.ToString().Contains(inicio.ToString()));
            if (fim != null && fim?.ToString().Trim() != "")
                result = result.Where(p => p.Fim.ToString().Contains(fim.ToString()));
            // if (funcionarios != null && funcionarios.Count() > 0)
            //     result = result.Where(p => !p.Funcionarios.Where(f => funcionarios.Contains(f.Id)).ConvertAll(f => funcionarios.Contains(f.Id)).Contains(false));
            int _inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - _inicio);
            result = result.ToList().GetRange(_inicio, qtd);
            
            return Json(new {size = _context.Projeto.Count(), entities = result});
        }
        public async Task<JsonResult> Get (int id) => Json(await _context.Projeto.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Nome,Inicio,Fim")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                Almoxarifado a = new Almoxarifado
                {
                    Nome = projeto.Nome,
                    Projeto = true,
                };
                projeto.Almoxarifado = a;
                _context.Add(projeto);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Inicio,Fim")] Projeto projeto)
        {
            if (id != projeto.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(projeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetoExists(projeto.Id))
                        return NotFound();
                    else
                        throw;
                }
                return Ok();
            }
            return BadRequest();
        }
        private bool ProjetoExists(int id) =>  _context.Projeto.Any(e => e.Id == id);
        public JsonResult ProjetoSelecionado(int? id) {
            return Json(_context.Projeto.FirstOrDefault(i=>i.Id==id));
        }
        public JsonResult FuncionariosNaoAlocados(int? id)
        {
            List<Funcionario> funcnaoalocados;
            List<int> funcalocados;
            funcalocados = _context.ProjetosxFuncionarios.Where(p => p.IdProjeto == id).Select(f => f.IdFuncionario).ToList();
            funcnaoalocados = _context.Funcionario.Where(f => !funcalocados.Contains(f.Id)).ToList();
            return Json(funcnaoalocados);
        }
        public JsonResult FuncionariosAlocados(int? id)
        {
            List<int> funcalocados;
            funcalocados = _context.ProjetosxFuncionarios.Where(p => p.IdProjeto == id).Select(f => f.IdFuncionario).ToList();
            return Json(_context.Funcionario.Where(f => funcalocados.Contains(f.Id)).ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AlocarFuncionario([Bind("Id,IdFuncionario,IdProjeto")] ProjetosxFuncionarios pxf)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pxf);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }
        public JsonResult FuncionarioNomes() {
            //return Json(_context.Funcionario.GetType().GetFields());
            return Json(new string[]{"ID","Nome","Cargo" });
        }
        public JsonResult Funcionarios()
        {
            //return Json(_context.Funcionario.GetType().GetFields());
            return Json(_context.Funcionario);
        }
        public JsonResult Funcionario(int? id) {
            return Json(_context.Funcionario.Find(id));
        }
        [HttpPost]
        public async Task<IActionResult> DesalocarProjeto(int? idfunc,int idproj)
        {
            var projeto = await _context.ProjetosxFuncionarios
                .FirstOrDefaultAsync(m => m.IdFuncionario == idfunc && m.IdProjeto == idproj);
            _context.ProjetosxFuncionarios.Remove(projeto);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Finalizar (int? id, DateTime? inicio, DateTime? fim)
        {
            if (id == null)
                return BadRequest("No id provided");
            if (inicio == null)
                return BadRequest("No start provided");
            if (fim == null)
                return BadRequest("No end provided");
            
            int _id = id.GetValueOrDefault();
            DateTime start = inicio.GetValueOrDefault();
            DateTime end = fim.GetValueOrDefault();

            if (start > end)
                return BadRequest("End must be after start");
            
            Projeto p = await _context.Projeto.FindAsync(_id);
            if (p == null)
                return BadRequest("Project not found");
            
            p.Fim = end;
            p.Almoxarifado.Ativo = false;

            _context.Update(p);
            await _context.SaveChangesAsync();
            
            return Ok();
        }
    }
}
