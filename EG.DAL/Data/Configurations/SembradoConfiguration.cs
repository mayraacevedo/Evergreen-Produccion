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
    public partial class SembradoConfiguration : IEntityTypeConfiguration<Sembrado>
    {
        public void Configure(EntityTypeBuilder<Sembrado> entity)
        {
            entity.HasOne(d => d.IdEstadoNavigation)
              .WithMany(p => p.Sembrados)
              .HasForeignKey(d => d.IdEstado)
              .OnDelete(DeleteBehavior.ClientSetNull);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Sembrado> entity);
    }
}
