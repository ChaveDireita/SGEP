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

namespace SGEP.Controllers
{
    [AllowAnonymous]
    public class FuncionarioController : Controller
    {
        public static string[] nomes = {"Fulano", "Sincrano", "Beltrano", "José Ninguém", "Funcionario"};
        public static string[] cargos = {"Pederiro", "Carpinteiro", "Faxineiro", "Bombeiro", "Professor", "Motorista", "Espadachim"};
        public static int next = 1;
        public static List<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
        private readonly ApplicationDbContext _context;

        public FuncionarioController(ApplicationDbContext context)
        {
            _context = context;
        }

        public JsonResult List([FromQuery] string id = "", [FromQuery] string nome = "", [FromQuery] string cargo = "")
        {
            //Task.Delay(1500).Wait();

            IEnumerable<Funcionario> result = Funcionarios;
            if (id != null && id.Trim() != "")
                result = result.Where(f => f.Id.ToString().Contains(id));
            if (nome != null && nome?.Trim() != "")
                result = result.Where(f => f.Nome.Contains(nome));
            if (cargo != null && cargo?.Trim() != "")
                result = result.Where(f => f.Cargo.Contains(cargo));

            return Json(result);
        }
        [HttpPost]
        public IActionResult Add()
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
            return Ok();
        }

        // GET: Funcionario
        public async Task<IActionResult> Index()
        {
            return View(/*await _context.Funcionario.ToListAsync()*/);
        }

        // GET: Funcionario/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // GET: Funcionario/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Funcionario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Cargo")] Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                _context.Add(funcionario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(funcionario);
        }

        public JsonResult ProjetosAssociados(int? id)
        {
            List<Projeto> projAss = (List<Projeto>) _context.ProjetosxFuncionarios
                .Where(f => f.FuncionarioAssociado.Id == id)
                .Select(p => p.ProjetoAssociado);
            return Json(projAss);
        }
        //public JsonResult ProjetoNomes()
        //{
            //return Json(new { titulos = string[] titulos, lista = _context.Model.ToList() };
        //}
            
        // GET: Funcionario/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var funcionario = await _context.Funcionario.FindAsync(id);
            if (funcionario == null)
            {
                return NotFound();
            }
            return View(funcionario);
        }

        // POST: Funcionario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Cargo")] Funcionario funcionario)
        {
            if (id != funcionario.Id)
            {
                return NotFound();
            }

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
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(funcionario);
        }

        // GET: Funcionario/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var funcionario = await _context.Funcionario
                .FirstOrDefaultAsync(m => m.Id == id);
            if (funcionario == null)
            {
                return NotFound();
            }

            return View(funcionario);
        }

        // POST: Funcionario/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
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
