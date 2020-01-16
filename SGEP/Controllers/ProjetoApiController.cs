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
    public class ProjetoApiController : Controller
    {
        public static int next = 1;
        public static List<Projeto> Projetos { get; set; } = new List<Projeto>();
        private readonly ApplicationDbContext _context;
        public ProjetoApiController(ApplicationDbContext context) => _context = context;
        public JsonResult List(string id, DateTime? inicio, DateTime? fim, int[] funcionarios, int? itensPorPagina, int? pagina)
        {
            Task.Delay(1500).Wait();
            IEnumerable<Projeto> result = Projetos;
            if (id != null && id.Trim() != "")
                result = result.Where(m => m.Id.ToString().Contains(id));
            // if (descricao != null && descricao?.Trim() != "")
            //     result = result.Where(m => m.Descricao.Contains(descricao));
            // if (preco != null && preco?.Trim() != "")
            //     result = result.Where(m => m.Preco.ToString().Contains(preco));
            // int inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            // int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - inicio);
            // result = result.ToList().GetRange(inicio, qtd);
            
            return Json(new {size = Projetos.Count(), entities = result});
        }
        [HttpPost]
        public IActionResult Add()
        {
            for (int i = 0; i < 50; i++)
            {
                if (new Random().Next()%10 == 0)
                {
                    Projetos.Add(new Projeto
                    {
                        Id = next++,
                        
                    });
                }
                else
                {
                    Projetos.Add(new Projeto
                    {
                        Id = next,
                    });
                }
            }
            return Ok();
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projeto = Projetos.First(p => p.Id == id);
            if (projeto == null)
                return NotFound();
            Projetos.Remove(projeto);
            //var funcionario = Funcionarios.First(e => e.Key == id && e.Value != null ).Value;//await _context.Funcionario.FindAsync(id);
            
            //_context.Funcionario.Remove(funcionario);
            //await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
