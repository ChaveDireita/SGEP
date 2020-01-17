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

        public ProjetoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projeto
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projeto.ToListAsync());
        }

        // GET: Projeto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projeto = await _context.Projeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }
        public JsonResult FuncionariosNaoAlocados(int? id)
        {

            List<Funcionario> funcnaoalocados = new List<Funcionario>(), funcalocados, functotais;
            funcalocados = FuncionariosAlocados(id);
            functotais = _context.Funcionario.ToList();
            foreach (Funcionario f in functotais)
            {
                if (!funcalocados.Contains(f))
                {
                    funcnaoalocados.Add(f);
                }
            }
            return Json(funcnaoalocados);
        }
        public List<Funcionario> FuncionariosAlocados(int? id)
        {
            List<Funcionario> funcalocados = (List<Funcionario>)_context.ProjetosxFuncionarios
                .Where(p => p.ProjetoAssociado.Id == id)
                .Select(p => p.FuncionarioAssociado);
            return funcalocados;
        }
        [HttpPost]
        public async Task<IActionResult> AlocarFuncionario([Bind("Id,FuncionarioAssociado,ProjetoAssociado")] ProjetosxFuncionarios pxf)
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
        // GET: Projeto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projeto/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nome,Inicio,Fim")] Projeto projeto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(projeto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(projeto);
        }

        // GET: Projeto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projeto = await _context.Projeto.FindAsync(id);
            if (projeto == null)
            {
                return NotFound();
            }
            return View(projeto);
        }

        // POST: Projeto/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nome,Inicio,Fim")] Projeto projeto)
        {
            if (id != projeto.Id)
            {
                return NotFound();
            }

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
            return View(projeto);
        }

        // GET: Projeto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projeto = await _context.Projeto
                .FirstOrDefaultAsync(m => m.Id == id);
            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

        // POST: Projeto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var projeto = await _context.Projeto.FindAsync(id);
            _context.Projeto.Remove(projeto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjetoExists(int id)
        {
            return _context.Projeto.Any(e => e.Id == id);
        }
    }
}
