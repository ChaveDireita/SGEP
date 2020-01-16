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
    public class FuncionarioApiController : Controller
    {
        public static string[] nomes = {"Fulano", "Sincrano", "Beltrano", "José Ninguém", "Funcionario"};
        public static string[] cargos = {"Pederiro", "Carpinteiro", "Faxineiro", "Bombeiro", "Professor", "Motorista", "Espadachim"};
        public static int next = 1;
        public static List<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
        private readonly ApplicationDbContext _context;
        public FuncionarioApiController(ApplicationDbContext context) => _context = context;
        public JsonResult List(string id, string nome, string cargo, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Funcionario> result = Funcionarios;
            if (id != null && id.Trim() != "")
                result = result.Where(f => f.Id.ToString().Contains(id));
            if (nome != null && nome?.Trim() != "")
                result = result.Where(f => f.Nome.Contains(nome));
            if (cargo != null && cargo?.Trim() != "")
                result = result.Where(f => f.Cargo.Contains(cargo));
            int inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - inicio);
            result = result.ToList().GetRange(inicio, qtd);
            
            return Json(new {size = Funcionarios.Count(), entities = result});
        }
        [HttpPost]
        public IActionResult Add()
        {
            for (int i = 0; i < 50; i++)
            {
                if (new Random().Next()%10 == 0)
                {
                    Funcionarios.Add(new Funcionario
                    {
                        Id = next++,
                        Nome = "Golero",
                        Cargo = "Bruno" 
                    });
                }
                else
                {
                    Funcionarios.Add(new Funcionario
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
            var funcionario = Funcionarios.First(f => f.Id == id);
            if (funcionario == null)
                return NotFound();
            Funcionarios.Remove(funcionario);
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
