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

        public JsonResult GraphData(DateTime? inicio, DateTime? fim, string tipo, int? material)
        {
            if (inicio == null || fim == null || material == null)
                return Json(new object[]{});

            MonthPeriod mpInicio = inicio.Value;
            MonthPeriod mpFim = fim.Value;

            var movs = _context.Movimentacao.Where(m => m.Tipo == tipo && 
                                                        m.Data >= inicio && 
                                                        m.Data <= fim);

            Dictionary<string, int> data = new Dictionary<string, int> {{"teste", 2}};
            foreach (var month in mpFim - mpInicio)
                data[month] = movs.Where(m => month == m.Data).Sum(m => m.Quantidade);
            
            return Json(data);
        }
    }
}