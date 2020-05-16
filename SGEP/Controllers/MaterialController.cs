﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGEP.Data;
using SGEP.Models;
using SGEP.Models.View;

namespace SGEP.Controllers
{
    [Authorize(Roles = "Almoxarife,Gerente")]
    public class MaterialController : Controller
    {
        private readonly ApplicationDbContext _context;
        public MaterialController(ApplicationDbContext context) => _context = context;

        public IActionResult Index() => View();

        public async Task<JsonResult> List(string codigo, string descricao, string preco, string unidade, int? itensPorPagina, int? pagina)
        {
            IEnumerable<MaterialAlmoxarifadoViewModel> result = ConverterListaParaShow(await _context.Material.ToListAsync());
            if (id != null && id.Trim() != "")
                result = result.Where(m => m.Id.ToString().Contains(id));
            if (descricao != null && descricao?.Trim() != "")
                result = result.Where(m => m.Descricao.Contains(descricao));
            if (preco != null && preco?.Trim() != "")
                result = result.Where(m => m.Preco.ToString().Contains(preco));
            int inicio = (itensPorPagina ?? 10)*((pagina ?? 1) - 1);
            int _size = result.Count();
            int qtd = Math.Min (itensPorPagina ?? 10, result.Count() - inicio);
            result = result.ToList().GetRange(inicio, qtd);
            
            return Json(new {size = _size, entities = result});
        }
        public JsonResult Unidades () => Json(_context.Unidade.ToList());
        public JsonResult UnidadePorIdJSON(int id) => Json(_context.Unidade.FirstOrDefault(u => u.Id == id));
        public IEnumerable<MaterialAlmoxarifadoViewModel> ConverterListaParaShow(IEnumerable<Material> mats)
        {
            List<MaterialAlmoxarifadoViewModel> matmodels = new List<MaterialAlmoxarifadoViewModel>();
            foreach(Material m in mats)
            {
                MaterialAlmoxarifadoViewModel matmodel = new MaterialAlmoxarifadoViewModel();
                Unidade un = _context.Unidade.FirstOrDefault(u => u.Id == m.IdUnidade);
                matmodel.Id = m.Id;
                matmodel.Preco = m.Preco;
                matmodel.Showid = m.Showid;
                if (un.Abreviacao == null) matmodel.PrecoUnidade = "R$" + m.Preco + "/" + un.Nome;
                else matmodel.PrecoUnidade = matmodel.PrecoUnidade = "R$" + m.Preco + "/" + un.Abreviacao;
                matmodel.Unidade = un.Nome;
                matmodel.Categoria = m.Categoria;
                matmodel.Descricao = m.Descricao;
                matmodels.Add(matmodel);
            }
            return matmodels;
        }
        public MaterialAlmoxarifadoViewModel GetViewModelMat(Material m)
        {
            MaterialAlmoxarifadoViewModel matmodel = new MaterialAlmoxarifadoViewModel();
            Unidade un = _context.Unidade.FirstOrDefault(u => u.Id == m.IdUnidade);
            matmodel.Id = m.Id;
            matmodel.Showid = m.Showid;
            if (un.Abreviacao == null) matmodel.PrecoUnidade = "R$" + m.Preco + "/" + un.Nome;
            else matmodel.PrecoUnidade = matmodel.PrecoUnidade = "R$" + m.Preco + "/" + un.Abreviacao;
            matmodel.Unidade = un.Nome;
            matmodel.Categoria = m.Categoria;
            matmodel.Preco = m.Preco;
            matmodel.Descricao = m.Descricao;
            return matmodel;
        }
        [HttpPost]
        public async Task<IActionResult> AdicionarUnidade([Bind("Id,Nome,Abreviacao")] Unidade unidade) {
            if (ModelState.IsValid)
            {
                _context.Add(unidade);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }
        [HttpGet("/Material/Unidade/Get/{id}")]
        public async Task<JsonResult> EditarUnidade (int id) => Json(await _context.Unidade.FirstOrDefaultAsync(u => u.Id==id));
        [HttpPost]
        public async Task<IActionResult> EditarUnidade(int id, [Bind("Id,Nome,Abreviacao")] Unidade unidade)
        {
            if (id != unidade.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(unidade);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        public async Task<JsonResult> All () => Json(await _context.Material.ToListAsync());

        [HttpGet("/Material/Get/{id}")]
        public async Task<JsonResult> Get (int id) => Json(GetViewModelMat(await _context.Material.FindAsync(id)));

        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Descricao,Preco,Categoria,IdUnidade")] Material material)
        {
            if (ModelState.IsValid)
            {
                Unidade un = _context.Unidade.FirstOrDefault(u => u.Id == material.IdUnidade);
                _context.Add(material);
                await _context.SaveChangesAsync();
                int qtdzero = 7 - material.Id.ToString().Length;
                string zeros = "";
                for (int i = 0; i < qtdzero; i++) zeros += "0";
                material.Showid = material.Categoria + "." + zeros + material.Id.ToString();
                _context.Update(material);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return BadRequest();
        }

        [Authorize(Roles = "Almoxarife")]
        [HttpPost]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Descricao,Categoria,Preco,IdUnidade,Showid")] Material material)
        {
            if (id != material.Id)
                return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.Id))
                        return NotFound();
                    else
                        throw;
                }
                return Ok();
            }
            return BadRequest();
        }
        private bool MaterialExists(int id) =>  _context.Material.Any(e => e.Id == id);
    }
}
