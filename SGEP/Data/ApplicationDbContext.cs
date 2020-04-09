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
        public DbSet<SGEPUser> User { get; set; }
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Projeto> Projeto { get; set; }
        public DbSet<Movimentacao> Movimentacao { get; set; }
        public DbSet<ProjetosxFuncionarios> ProjetosxFuncionarios { get; set; }
    }
}
