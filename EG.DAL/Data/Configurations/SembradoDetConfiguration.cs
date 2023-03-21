using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EG.Models.Entities;

namespace EG.DAL.Data.Configurations
{
    public partial class SembradoDetConfiguration : IEntityTypeConfiguration<SembradoDet>
    {
        public void Configure(EntityTypeBuilder<SembradoDet> entity)
        {
            entity.HasOne(d => d.IdSembradoNavigation)
               .WithMany(p => p.SembradosDets)
               .HasForeignKey(d => d.IdSembrado)
               .OnDelete(DeleteBehavior.ClientCascade);


            entity.HasOne(d => d.IdParcelaNavigation)
                .WithMany(p => p.SembradosDets)
                .HasForeignKey(d => d.IdParcela)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdSemillaNavigation)
                .WithMany(p => p.SembradosDets)
                .HasForeignKey(d => d.IdSemilla)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(d => d.IdEstadoNavigation)
              .WithMany(p => p.SembradosDets)
              .HasForeignKey(d => d.IdEstado)
              .OnDelete(DeleteBehavior.ClientSetNull);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<SembradoDet> entity);
    }
}
