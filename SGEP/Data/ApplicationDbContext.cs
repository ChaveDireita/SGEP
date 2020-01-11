using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SGEP.Models;

namespace SGEP.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base (options)
        {
        }
        public DbSet<SGEP.Models.Funcionario> Funcionario { get; set; }
        public DbSet<SGEP.Models.Material> Material { get; set; }
        public DbSet<SGEP.Models.Projeto> Projeto { get; set; }
        public DbSet<SGEP.Models.Movimentacao> Movimentacao { get; set; }
    }
}
