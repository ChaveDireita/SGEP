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
    public class ProjetoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public ProjetoController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();
        public async Task<JsonResult> List(string codigo, string nome, DateTime? inicio, DateTime? fim, string estado, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Projeto> result = await _context.Projeto.Include(p => p.Almoxarifado).ToListAsync();
            if (codigo != null && codigo.Trim() != "")
                result = result.Where(p => p.Id.ToString().Contains(codigo));
            if (nome != null && nome.Trim() != "")
                result = result.Where(p => p.Nome.Contains(nome));
            if (inicio != null && inicio?.ToString().Trim() != "")
                result = result.Where(p => p.Inicio.ToString().Contains(inicio.ToString()));
            if (fim != null && fim?.ToString().Trim() != "")
                result = result.Where(p => p.Fim.ToString().Contains(fim.ToString()));
            if (estado != null && estado?.ToString().Trim() != "")
                result = result.Where(p => (p.Fim == null) == bool.Parse(estado));
            int _inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - _inicio);
            int _size = result.Count();
            result = result.ToList().GetRange(_inicio, qtd);
            
            return Json(new {size = _size, entities = result.ToList()
                                                            .ConvertAll(p => new {
                                                                p.Id, Inicio = p.Inicio.ToShortDateString(), 
                                                                Fim = (p.Fim == null) ? "--" : p.Fim.GetValueOrDefault().ToShortDateString(), 
                                                                p.Almoxarifado, 
                                                                p.Nome})});
        }
        public async Task<JsonResult> Get (int id) => Json(await _context.Projeto.FindAsync(id));
        [Authorize(Roles = "Almoxarife")]
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
                return Ok("Projeto adicionado com sucesso.");
            }
            return BadRequest("Ocorreu um erro ao adicionar o projeto.");
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Inicio,Fim,AlmoxarifadoId")] Projeto projeto)
        {
            if (id != projeto.Id)
                return NotFound("O projeto não existe.");
            if (ModelState.IsValid)
            {
                try
                {
                    projeto.Almoxarifado = await _context.Almoxarifado.FindAsync(projeto.AlmoxarifadoId);
                    projeto.Almoxarifado.Nome = projeto.Nome;
                    _context.Update(projeto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjetoExists(projeto.Id))
                        return NotFound("O projeto não existe.");
                    else
                        throw;
                }
                return Ok("As alterações foram salvas com sucesso.");
            }
            return BadRequest("Ocorreu um erro ao salvar as alterações.");
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
            funcnaoalocados = _context.Funcionario.Where(f => f.Ativo && !funcalocados.Contains(f.Id)).ToList();
            return Json(funcnaoalocados);
        }
        public JsonResult FuncionariosAlocados(int? id)
        {
            List<int> funcalocados;
            funcalocados = _context.ProjetosxFuncionarios.Where(p => p.IdProjeto == id).Select(f => f.IdFuncionario).ToList();
            return Json(_context.Funcionario.Where(f => funcalocados.Contains(f.Id)).ToList());
        }
        [Authorize(Roles = "Almoxarife")]
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
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> DesalocarProjeto(int? idfunc,int idproj)
        {
            var projeto = await _context.ProjetosxFuncionarios
                .FirstOrDefaultAsync(m => m.IdFuncionario == idfunc && m.IdProjeto == idproj);
            _context.ProjetosxFuncionarios.Remove(projeto);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> Finalizar (int? id, DateTime? inicio, DateTime? fim)
        {
            if (id == null)
                return BadRequest("Este projeto não existe.");
            if (inicio == null || fim == null)
                return BadRequest("Ocorreu um erro ao enviar os dados ao servidor.");
            int _id = id.GetValueOrDefault();
            DateTime start = inicio.GetValueOrDefault();
            DateTime end = fim.GetValueOrDefault();

            if (start > end)
                return BadRequest("A data final do projeto deve ser posterior a sua data inicial.");
            
            Projeto p = await _context.Projeto.FindAsync(_id);
            if (p == null)
                return BadRequest("Projeto não existe.");
            if (p.Almoxarifado.AlmoxarifadosxMateriais.Count > 0)
                return BadRequest("Existem materiais alocados no projeto.");

            p.Fim = end;
            p.Almoxarifado.Ativo = false;

            _context.Update(p);
            await _context.SaveChangesAsync();

            return Ok("Projeto finalizado com successo!");
        }

        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public JsonResult ValidateDate(DateTime inicio, DateTime fim)
        {
            if (fim < inicio)
                return Json("A data de término deve ser posterior a data de início.");
            return Json(true);
        }
    }
}
