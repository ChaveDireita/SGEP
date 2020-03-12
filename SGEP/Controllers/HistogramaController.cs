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
    public class HistogramaController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HistogramaController(ApplicationDbContext context) => _context = context;
        public IActionResult Index() => View();
        public JsonResult GraphData(string inicio, string fim, string tipo, int? material, int? almoxarifado)
        {
            if (inicio == null || fim == null || material == null)
                return Json(new object[]{});

            MonthPeriod mpInicio = inicio;
            MonthPeriod mpFim = fim;

            var movs = _context.Movimentacao.Where(m => m.Tipo.Equals(tipo, StringComparison.InvariantCultureIgnoreCase) && 
                                                        m.Data >= mpInicio && 
                                                        m.Data <= mpFim &&
                                                        m.MaterialId == material.Value);
            
            if (almoxarifado != null)
            {
                if (tipo.Equals("entrada", StringComparison.InvariantCultureIgnoreCase))
                    movs = movs.Where(m => m.DestinoId == almoxarifado);
                else
                    movs = movs.Where(m => m.OrigemId == almoxarifado);
            }
            
            movs.Where(m => m.Id == 2);                                                        

            Dictionary<string, int> data = new Dictionary<string, int> ();
            IEnumerable<MonthPeriod> months = mpFim - mpInicio;
            foreach (var month in months)
                data[month] = movs.Where(m => month == ((MonthPeriod) m.Data))
                                  .Sum(m => m.Quantidade);
            
            string almoxarifadoNome = null;
            if (almoxarifado != null)
                almoxarifadoNome = _context.Almoxarifado.Find(almoxarifado).Nome;

            return Json(new { data, material, tipo, almoxarifado = almoxarifadoNome });
        }
    }
}