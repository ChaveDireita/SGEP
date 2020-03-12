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
        public JsonResult GraphData(string inicio, string fim, string tipo, int? material)
        {
            if (inicio == null || fim == null || material == null)
                return Json(new object[]{});

            MonthPeriod mpInicio = inicio;
            MonthPeriod mpFim = fim;

            var movs = _context.Movimentacao.Where(m => m.Tipo.Equals(tipo, StringComparison.InvariantCultureIgnoreCase) && 
                                                        m.Data >= mpInicio && 
                                                        m.Data <= mpFim &&
                                                        m.MaterialId == material.Value);
            movs.Where(m => m.Id == 2);                                                        

            Dictionary<string, int> data = new Dictionary<string, int> ();
            IEnumerable<MonthPeriod> months = mpFim - mpInicio;
            foreach (var month in months)
            {
                data[month] = 0;
                
                int a = movs.Where(m => month == ((MonthPeriod) m.Data))
                            .Sum(m => m.Quantidade);
                data[month] = a;
            }
            
            return Json(new { data, material = "PLACEHOLDER", tipo });
        }
    }
}