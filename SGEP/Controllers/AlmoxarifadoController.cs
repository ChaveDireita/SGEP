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
    [AllowAnonymous]
    public class AlmoxarifadoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public AlmoxarifadoController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();
        public async Task<JsonResult> List(string id, string nome, string projeto, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Almoxarifado> result = await _context.Almoxarifado.ToListAsync();
            if (id != null && id.Trim() != "")
                result = result.Where(m => m.Id.ToString().Contains(id));
            if (nome != null && nome?.Trim() != "")
                result = result.Where(m => m.Nome.Contains(nome));
            if (projeto != null && projeto?.Trim() != "")
                result = result.Where(m => m.Projeto.ToString().Contains(projeto));
            int inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - inicio);
            result = result.ToList().GetRange(inicio, qtd);
            
            return Json(new {size = _context.Almoxarifado.Count(), entities = result.ToList().ConvertAll(a => new { a.Id, a.Nome, a.Projeto })});
        }

        public async Task<JsonResult> All() => Json((await _context.Almoxarifado.ToListAsync()).ConvertAll(a => new { a.Id, a.Nome, a.Projeto }));
        [HttpGet("/Almoxarifado/GetMateriais/{id}")]
        public async Task<JsonResult> GetMateriais(int id)
        {
            var almoxarifadoMateriais = (await _context.Almoxarifado.FindAsync(id)).AlmoxarifadosxMateriais;
            var materiais = (await _context.Material.Where(m => almoxarifadoMateriais.ConvertAll(am => am.MaterialId)
                                                                              .Contains(m.Id))
                                                                              .ToListAsync())
                                                                              .ConvertAll(m => new { m.Id, m.Descricao, m.Categoria, m.Preco});
            return Json(materiais);
        } 

        [HttpGet("/Almoxarifado/GetQuantidade/{idAlm}/{idMat}")]
        public async Task<JsonResult> GetMateriais(int idAlm, int idMat) => Json ((await _context.Almoxarifado
                                                                                                 .FindAsync(idAlm))
                                                                                                 .AlmoxarifadosxMateriais
                                                                                                 .Where(am => am.MaterialId == idMat)
                                                                                                 .ToList()
                                                                                                 .ConvertAll(am => new { material = am.MaterialId, quantidade = am.Quantidade}));
            

        [HttpGet("/Almoxarifado/Get/{id}")]
        public async Task<JsonResult> Get (int id)
        {
            Almoxarifado a = await _context.Almoxarifado.FindAsync(id);
            //List<AlmoxarifadosxMateriais> materiais = await _context.
            return Json(new {almoxarifado = a});
        } 
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Nome,Projeto")] Almoxarifado almoxarifado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(almoxarifado);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Projeto")] Almoxarifado almoxarifado)
        {
            if (id != almoxarifado.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(almoxarifado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlmoxarifadoExists(almoxarifado.Id))
                        return NotFound();
                    else
                        throw;
                }
                return Ok();
            }
            return BadRequest();
        }
        
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var almoxarifado = await _context.Almoxarifado.FirstAsync(a => a.Id == id);
            if (almoxarifado == null)
                return NotFound();
            if (almoxarifado.Projeto)
                return BadRequest();
            _context.Almoxarifado.Remove(almoxarifado);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private bool AlmoxarifadoExists(int id) =>  _context.Almoxarifado.Any(e => e.Id == id);
    }
}
