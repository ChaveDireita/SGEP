using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SGEP.Data;
using SGEP.Models;

namespace SGEP.Controllers
{
    [AllowAnonymous]
    public class DebugController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<SGEPUser> _userManager;
        public DebugController(ApplicationDbContext context, UserManager<SGEPUser> userManager) 
        { 
            _context = context;
            _userManager = userManager;
        }
        
        public JsonResult Users()
        {
            return Json(new {_userManager.Users, count = _userManager.Users.Count()});
        }
        
    }
}