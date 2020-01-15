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
    public class MaterialApiController : Controller
    {
        public static int next = 1;
        public static List<Material> Materiais { get; set; } = new List<Material>();
        private readonly ApplicationDbContext _context;
        public MaterialApiController(ApplicationDbContext context) => _context = context;
        public JsonResult List(string id, string descricao, string preco, , int? itensPorPagina, int? pagina)
        {
            Task.Delay(1500).Wait();
            IEnumerable<Material> result = Materiais;
            if (id != null && id.Trim() != "")
                result = result.Where(m => m.Id.ToString().Contains(id));
            if (descricao != null && descricao?.Trim() != "")
                result = result.Where(m => m.Descricao.Contains(descricao));
            if (preco != null && preco?.Trim() != "")
                result = result.Where(m => m.Preco.ToString().Contains(preco));
            int inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - inicio);
            result = result.ToList().GetRange(inicio, qtd);
            
            return Json(new {size = Materiais.Count(), entities = result});
        }
        [HttpPost]
        public IActionResult Add()
        {
            for (int i = 0; i < 50; i++)
            {
                if (new Random().Next()%10 == 0)
                {
                    Materiais.Add(new Funcionario
                    {
                        Id = next++,
                        Nome = "Golero",
                        Cargo = "Bruno" 
                    });
                }
                else
                {
                    Materiais.Add(new Funcionario
                    {
                        Id = next,
                        Nome = nomes[new Random().Next()%nomes.Length] + next,
                        Cargo = cargos[new Random().Next()%cargos.Length] + next++ 
                    });
                }
            }
            return Ok();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var funcionario = Materiais.First(f => f.Id == id);
            if (funcionario == null)
                return NotFound();
            Materiais.Remove(funcionario);
            //var funcionario = Funcionarios.First(e => e.Key == id && e.Value != null ).Value;//await _context.Funcionario.FindAsync(id);
            
            //_context.Funcionario.Remove(funcionario);
            //await _context.SaveChangesAsync();
            return Ok();//RedirectToAction(nameof(Index));
        }

        private bool FuncionarioExists(int id)
        {
            return _context.Funcionario.Any(e => e.Id == id);
        }
    }
}
