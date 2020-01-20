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
    public class MovimentacaoApiController : Controller
    {
        public static int next = 1;
        public static List<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
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
        public MovimentacaoApiController(ApplicationDbContext context) => _context = context;
        public JsonResult List(DateTime? data, string origem, string destino, string material, int? quantidade, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Movimentacao> result = Movimentacoes;
            // if (nome != null && nome.Trim() != "")
            //     result = result.Where(p => p.Nome.Contains(nome));
            // if (inicio != null && inicio?.ToString().Trim() != "")
            //     result = result.Where(p => p.Inicio.ToString().Contains(inicio.ToString()));
            // if (fim != null && fim?.ToString().Trim() != "")
            //     result = result.Where(p => p.Fim.ToString().Contains(fim.ToString()));
            // if (funcionarios != null && funcionarios.Count() > 0)
            //     result = result.Where(p => !p.Funcionarios.Where(f => funcionarios.Contains(f.Id)).ConvertAll(f => funcionarios.Contains(f.Id)).Contains(false));
            int _inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - _inicio);
            result = result.ToList().GetRange(_inicio, qtd);
            
            return Json(new {size = Movimentacoes.Count(), entities = result});
        }
        [HttpPost]
        public IActionResult Add()
        {
            
            for (int i = 0; i < 50; i++)
            {
                string tipo = new string[] {"Entrada", "Saída", "Consumo"}[new Random().Next()%3];
                Movimentacao m = new Movimentacao
                {
                    Id = next++,
                    Data = DateTime.Now,
                    Tipo = tipo,
                    Quantidade = new Random().Next(),
                    MaterialId = new Random().Next()
                };

                if (m.Tipo != "Entrada")
                    m.Origem = new Almoxarifado { Id = new Random().Next() };
                if (m.Tipo != "Consumo")
                    m.Destino = new Almoxarifado { Id = new Random().Next() };

                Movimentacoes.Add(m);
            }
            return Ok();
        }
    }
}
