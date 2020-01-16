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
        string[] descricoes = 
        {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Duis molestie cursus augue, nec lobortis mi suscipit quis. Suspendisse egestas ex sed suscipit dictum. Nullam dictum vehicula tortor, nec rhoncus quam condimentum sed. Mauris laoreet metus ac nisi euismod, vitae sagittis.",
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nullam egestas eget augue eu posuere. Nullam placerat, leo ac pharetra ultrices, nibh quam faucibus massa, et euismod elit metus sed leo. Curabitur elementum elit augue. Duis ac risus a velit facilisis."
        };
        public static List<Material> Materiais { get; set; } = new List<Material>();
        private readonly ApplicationDbContext _context;
        public MaterialApiController(ApplicationDbContext context) => _context = context;
        public JsonResult List(string id, string descricao, string preco, string categoria, int? itensPorPagina, int? pagina)
        {
            //Task.Delay(1500).Wait();
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
                Materiais.Add(new Material
                {
                    Id = next++,
                    Preco = (decimal) (new Random().NextDouble()*1000000000.0),
                    Descricao = descricoes[new Random().Next()%descricoes.Count()]
                });
            }
            return Ok();
        }
    }
}
