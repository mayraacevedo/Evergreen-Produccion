using EG.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.DAL.Data
{
    public partial class EGDbContextBase : DbContext
    {
        public EGDbContextBase()
        {

        }

        public EGDbContextBase(DbContextOptions options) : base(options)
        {
        }

        public virtual DbSet<Semilla> Semillas { get; set; } = default!;
        public virtual DbSet<TipoSemilla> TiposSemillas { get; set; } = default!;
        public virtual DbSet<Parcela> Parcelas { get; set; } = default!;
        public virtual DbSet<EstadoSembrado> EstadosSembrados { get; set; } = default!;
        public virtual DbSet<EstadoSembradoParcela> EstadosSembradosParcelas { get; set; } = default!;
        public virtual DbSet<Sembrado> Sembrados { get; set; } = default!;
        public virtual DbSet<SembradoDet> SembradosDet { get; set; } = default!;
        public virtual DbSet<ConsecutivoData> Consecutivos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.ApplyConfiguration(new Configurations.SembradoConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SemillaConfiguration());
            modelBuilder.ApplyConfiguration(new Configurations.SembradoDetConfiguration());

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
