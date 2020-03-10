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
        public DbSet<Funcionario> Funcionario { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<Projeto> Projeto { get; set; }
        public DbSet<Movimentacao> Movimentacao { get; set; }
        public DbSet<ProjetosxFuncionarios> ProjetosxFuncionarios { get; set; }
        public DbSet<Almoxarifado> Almoxarifado { get; set; }
        public DbSet<AlmoxarifadosxMateriais> AlmoxarifadosxMateriais { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<AlmoxarifadosxMateriais>()
                   .HasKey(am => new {am.AlmoxarifadoId, am.MaterialId});
            builder.Entity<AlmoxarifadosxMateriais>()
                   .HasOne(am => am.Almoxarifado)
                   .WithMany(a => a.AlmoxarifadosxMateriais)
                   .HasForeignKey(am => am.AlmoxarifadoId);
            builder.Entity<AlmoxarifadosxMateriais>()
                   .HasOne(am => am.Material)
                   .WithMany(m => m.AlmoxarifadosxMateriais)
                   .HasForeignKey(am => am.MaterialId);
            
            //builder
        }
    }
}
