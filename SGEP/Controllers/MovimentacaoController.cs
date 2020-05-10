﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SGEP.Data;
using SGEP.Models;

namespace SGEP.Controllers
{
    [Authorize(Roles = "Almoxarife,Gerente")]
    public class MovimentacaoController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MovimentacaoController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();
        public JsonResult List(DateTime? data, string origem, string destino, string material, string tipo, int? itensPorPagina, int? pagina)
        {
            IEnumerable<Movimentacao> result = _context.Movimentacao;
            if (data != null)
                result = result.Where(m => m.Data == data);
            if (origem != null && origem?.ToString().Trim() != "")
                result = result.Where(m => {
                    _context.Almoxarifado.Find(m.OrigemId);
                    var alm = _context.Almoxarifado.Find(m.OrigemId);
                    return alm != null && alm.Nome.Contains(origem);
                });
            if (destino != null && destino?.ToString().Trim() != "")
                result = result.Where(m => {
                    _context.Almoxarifado.Find(m.DestinoId);
                    var alm = _context.Almoxarifado.Find(m.DestinoId);
                    return alm != null && alm.Nome.Contains(destino);
                });
            if (material != null && material?.ToString().Trim() != "")
                result = result.Where(m => m.MaterialId.ToString().Contains(material.ToString()));
            if (tipo != null && tipo?.ToString().Trim() != "")
                result = result.Where(m => m.Tipo.Contains(tipo));
            int _inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - _inicio);
            result = result.OrderByDescending(m => m.Data).ToList().GetRange(_inicio, qtd);
            var result2 = result.ToList().ConvertAll(m => new { 
                    m.Id, 
                    Data = m.Data.ToShortDateString(), 
                    m.MaterialId, 
                    m.Preco, 
                    m.Quantidade, 
                    m.Tipo, 
                    Origem = _context.Almoxarifado.Find(m.OrigemId),
                    Destino = _context.Almoxarifado.Find(m.DestinoId)
                });
            return Json(new {size = _context.Movimentacao.Count(), entities = result2});
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> CreateEntrada([Bind("Data", "MaterialId", "Quantidade", "DestinoId", "Tipo")] Movimentacao movimentacao)
        {
            Almoxarifado destino = await _context.Almoxarifado.FindAsync(movimentacao.DestinoId);
            if (movimentacao.Quantidade <= 0)
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
            return Ok("Movimentação adicionada com sucesso.");
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> CreateSaida([Bind("Data", "MaterialId", "Quantidade", "OrigemId","DestinoId", "Tipo")] Movimentacao movimentacao)
        {
            Almoxarifado origem = await _context.Almoxarifado.FindAsync(movimentacao.OrigemId);
            Almoxarifado destino = await _context.Almoxarifado.FindAsync(movimentacao.DestinoId);
            if (movimentacao.Quantidade <= 0)
                return BadRequest("Quantidade não pode ser menor que 0");
            if (!movimentacao.Tipo.Equals("saída", StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("O tipo de ser \"saída\"");
            if (destino == null)
                return BadRequest("O destino não existe");
            if (origem == null)
                return BadRequest("A origem não existe");
            _context.Add(movimentacao);
            var destinoAlmoxarifadoMaterial = destino.AlmoxarifadosxMateriais.Find(am => am.MaterialId == movimentacao.MaterialId);
            var origemAlmoxarifadoMaterial = origem.AlmoxarifadosxMateriais.Find(am => am.MaterialId == movimentacao.MaterialId);

            origemAlmoxarifadoMaterial.Quantidade -= movimentacao.Quantidade;
            if (origemAlmoxarifadoMaterial.Quantidade == 0)
                origem.AlmoxarifadosxMateriais.Remove(origemAlmoxarifadoMaterial);
            if (destinoAlmoxarifadoMaterial == null)
                destino.AlmoxarifadosxMateriais.Add(new AlmoxarifadosxMateriais{AlmoxarifadoId = destino.Id, MaterialId = movimentacao.MaterialId, Quantidade = movimentacao.Quantidade});
            else
                destinoAlmoxarifadoMaterial.Quantidade += movimentacao.Quantidade;
            _context.UpdateRange(origem, destino);
            await _context.SaveChangesAsync();
            return Ok("Movimentação adicionada com sucesso.");
        }
        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> CreateConsumo([Bind("Data", "MaterialId", "Quantidade", "OrigemId", "Tipo")] Movimentacao movimentacao)
        {
            Almoxarifado origem = await _context.Almoxarifado.FindAsync(movimentacao.OrigemId);
            if (movimentacao.Quantidade <= 0 || movimentacao.Quantidade > origem.AlmoxarifadosxMateriais.Find(am => am.MaterialId == movimentacao.MaterialId).Quantidade)
                return BadRequest("Quantidade não pode ser menor que 0");
            if (!movimentacao.Tipo.Equals("consumo", StringComparison.InvariantCultureIgnoreCase))
                return BadRequest("O tipo de ser \"consumo\"");
            if (origem == null)
                return BadRequest("A origem não existe");
            _context.Add(movimentacao);
            var almoxarifadoMaterial = origem.AlmoxarifadosxMateriais.Find(am => am.MaterialId == movimentacao.MaterialId);
            almoxarifadoMaterial.Quantidade -= movimentacao.Quantidade;
            if (almoxarifadoMaterial.Quantidade == 0)
                origem.AlmoxarifadosxMateriais.Remove(almoxarifadoMaterial);
            _context.Update(origem);
            await _context.SaveChangesAsync();
            return Ok("Movimentação adicionada com sucesso.");
        }
    }
}
