using EG.Models.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EG.DAL.Data.Configurations
{
    public partial class SemillaConfiguration : IEntityTypeConfiguration<Semilla>
    {
        public void Configure(EntityTypeBuilder<Semilla> entity)
        {
            entity.HasOne(d => d.IdTipoSemillaNavigation)
                .WithMany(p => p.Semillas)
                .HasForeignKey(d => d.IdTipoSemilla)
                .OnDelete(DeleteBehavior.ClientSetNull);

            OnConfigurePartial(entity);
        }

        partial void OnConfigurePartial(EntityTypeBuilder<Semilla> entity);
    }
}
