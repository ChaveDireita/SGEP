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
    public class MovimentacaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MovimentacaoController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();
        public JsonResult List(DateTime? data, string origem, string destino, string material, int? quantidade, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Movimentacao> result = _context.Movimentacao;
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
            result = result.OrderByDescending(m => m.Id).ToList().GetRange(_inicio, qtd);
            
            return Json(new {size = _context.Movimentacao.Count(), entities = result});
        }
        [HttpPost]
        public async Task<IActionResult> CreateEntrada([Bind("Data", "MaterialId", "Quantidade", "DestinoId", "Tipo")] Movimentacao movimentacao)
        {
            Almoxarifado destino = await _context.Almoxarifado.FindAsync(movimentacao.DestinoId);
            if (movimentacao.Quantidade < 0)
                return BadRequest("Quantidade não pode ser menor que 0");
            if (!movimentacao.Tipo.Equals("entrada", StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("O tipo de ser \"entrada\"");
            if (destino == null)
                return BadRequest("O destino não existe");
            _context.Add(movimentacao);
            var almoxarifadoMaterial = destino.AlmoxarifadosxMateriais.Find(am => am.MaterialId == movimentacao.MaterialId);
            if (almoxarifadoMaterial == null)
                destino.AlmoxarifadosxMateriais.Add(new AlmoxarifadosxMateriais{AlmoxarifadoId = destino.Id, MaterialId = movimentacao.MaterialId, Quantidade = movimentacao.Quantidade});
            else
                almoxarifadoMaterial.Quantidade += movimentacao.Quantidade;
            _context.Update(destino);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
