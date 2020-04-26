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
    public class MaterialController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MaterialController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();
        public async Task<JsonResult> List(string id, string descricao, string preco, string categoria, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Material> result = await _context.Material.ToListAsync();
            if (id != null && id.Trim() != "")
                result = result.Where(m => m.Id.ToString().Contains(id));
            if (descricao != null && descricao?.Trim() != "")
                result = result.Where(m => m.Descricao.Contains(descricao));
            if (preco != null && preco?.Trim() != "")
                result = result.Where(m => m.Preco.ToString().Contains(preco));
            int inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - inicio);
            result = result.ToList().GetRange(inicio, qtd);
            
            return Json(new {size = _context.Material.Count(), entities = result});
        }
        public JsonResult Unidades () => Json(_context.Unidade.ToList());
        [HttpPost]
        public async Task<IActionResult> AdicionarUnidade([Bind("Id,Nome,Abreviacao")] Unidade unidade) {
            if (ModelState.IsValid)
            {
                _context.Add(unidade);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> EditarUnidade(int id, [Bind("Id,Nome,Alocacao")] Unidade unidade)
        {
            if (id != unidade.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(unidade);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        
        public async Task<JsonResult> All () => Json(await _context.Material.ToListAsync());
        [HttpGet("/Material/Get/{id}")]
        public async Task<JsonResult> Get (int id) => Json(await _context.Material.FindAsync(id));
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Preco")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Preco")] Material material)
        {
            if (id != material.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
                        return NotFound();
                    else
                        throw;
                }
                return Ok();
            }
            return BadRequest();
        }
        private bool MaterialExists(int id) =>  _context.Material.Any(e => e.Id == id);
    }
}
