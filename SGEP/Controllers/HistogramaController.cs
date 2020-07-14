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
    [Authorize(Roles = "Almoxarife,Gerente")]
    public class HistogramaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HistogramaController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();
        ///<summary>
        ///Retorna os dados das movimentações para serem exibidos no histograma em Views/Histograma/Index.cshtml
        ///</summary>
        public JsonResult GraphData(string inicio, string fim, string tipo, int? material, int? almoxarifado)
        {
            if (inicio == null || fim == null || material == null || tipo == null || almoxarifado == null)
                return Json(new object[]{});
            
            MonthPeriod mpInicio = inicio;
            MonthPeriod mpFim = fim;

            var movs = _context.Movimentacao.Where(m => m.Tipo.Equals(tipo, StringComparison.InvariantCultureIgnoreCase) && 
                                                        m.Data >= mpInicio && 
                                                        m.Data <= mpFim &&
                                                        m.MaterialId == material.Value);
            
            if (almoxarifado.GetValueOrDefault() != -1)
            {
                if (tipo.Equals("entrada", StringComparison.InvariantCultureIgnoreCase))
                    movs = movs.Where(m => m.DestinoId == almoxarifado);
                else
                    movs = movs.Where(m => m.OrigemId == almoxarifado);
            }
            
            Dictionary<string, decimal> data = new Dictionary<string, decimal> ();
            IEnumerable<MonthPeriod> months = mpFim - mpInicio;
            foreach (var month in months)
                data[month] = movs.Where(m => month == ((MonthPeriod) m.Data))
                                  .Sum(m => m.Quantidade);
            
            string almoxarifadoNome = null;
            if (almoxarifado != -1)
                almoxarifadoNome = _context.Almoxarifado.Find(almoxarifado).Nome;

            string title = null;
            string materialShowId = _context.Material.Find(material.GetValueOrDefault()).Showid;

            switch (tipo)
            {
                case "Entrada":
                    title = almoxarifadoNome != null ? $"Entrada de {materialShowId} para {almoxarifadoNome}" 
                                                     : $"Entrada total de {materialShowId}"; 
                    break;
                case "Saida":
                    title = almoxarifadoNome != null ? $"Saída de {materialShowId} de {almoxarifadoNome}" 
                                                     : $"Saída total de {materialShowId}"; 
                    break;
                case "Consumo":
                    title = almoxarifadoNome != null ? $"Consumo de {materialShowId} por {materialShowId}" 
                                                     : $"Consumo total de {materialShowId}"; 
                    break;
            }

            return Json(new { data, materialShowId, tipo, almoxarifado = almoxarifadoNome, title });
        }
        ///<summary>
        ///Verifica se os meses inicial e final são válidos no formulário Views/Histograma/Partial/_Form.cshtml
        ///É usado para validação remota.
        ///</summary>
        [AllowAnonymous]
        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateDate(DateTime? inicio, DateTime? fim)
        {
            if (inicio == null || fim == null)
                return Json(true);
            if (fim.GetValueOrDefault() < inicio.GetValueOrDefault())
                return Json("A data final deve ser posterior a data inicial.");
            return Json(true);
        }
    }
}