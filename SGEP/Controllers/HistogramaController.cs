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
        public HistogramaController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {

            return View();
        }

        public JsonResult GraphData(string inicio, string fim, string tipo, int? material)
        {
            // if (tipo == null)
            //     throw new ArgumentException("tipo is null");
            // if (material == null)
            //     throw new ArgumentException("material is null");
            // if (inicio == null)
            //     throw new ArgumentException("inicio is null");
            // if (fim == null)
            //     throw new ArgumentException("fim is null");
            if (inicio == null || fim == null || material == null)
                return Json(new object[]{});

            MonthPeriod mpInicio = inicio;
            MonthPeriod mpFim = fim;

            var movs = new List<Movimentacao>().Where(m => m.Tipo == tipo && 
                                                        (MonthPeriod) m.Data >= inicio && 
                                                        (MonthPeriod) m.Data <= fim &&
                                                        m.MaterialId == material.Value);

            Dictionary<string, int> data = new Dictionary<string, int> ();
            IEnumerable<MonthPeriod> months = mpFim - mpInicio;
            foreach (var month in months)
                data[month] = movs.Where(m => month == m.Data).Sum(m => m.Quantidade);
            
            return Json(new { data, material = "PLACEHOLDER", tipo });
        }
    }
}