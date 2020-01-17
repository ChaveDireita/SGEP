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
        public static List<string> Nomes { get; set; } = new List<string>
        {
            "Projeto A",
            "Projeto B",
            "Projeto C",
            "CIMATEC PARK",
            "PFC",
            "Bolo"
        };
        private readonly ApplicationDbContext _context;
        public ProjetoApiController(ApplicationDbContext context) => _context = context;
        public JsonResult List(string id, string nome, DateTime? inicio, DateTime? fim, int[] funcionarios, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Projeto> result = Projetos;
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
            
            return Json(new {size = Projetos.Count(), entities = result});
        }
        [HttpPost]
        public IActionResult Add()
        {
            for (int i = 0; i < 50; i++)
            {
                Projetos.Add(new Projeto
                {
                    Id = next++,
                    Inicio = DateTime.Now,
                    Fim = DateTime.Now,
                    Nome = Nomes[new Random().Next()%Nomes.Count()]
                });
            }
            return Ok();
        }
    }
}
