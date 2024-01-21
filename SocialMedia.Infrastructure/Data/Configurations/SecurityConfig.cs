using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SocialMedia.Core.Entities;
using SocialMedia.Core.Enumerations;

namespace SocialMedia.Infrastructure.Data.Configurations
{
    public class SecurityConfig : IEntityTypeConfiguration<Security>
    {
        public void Configure(EntityTypeBuilder<Security> builder)
        {

            builder.ToTable("Seguridad");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("IdSeguridad");

            builder.Property(e => e.User)
                .HasColumnName("Usuario")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(e => e.UserName)
                .HasColumnName("NombreUsuario")
                .HasMaxLength(50)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Password)
                .HasColumnName("Contrasena")
                .HasMaxLength(200)
                .IsRequired()
                .IsUnicode(false);

            builder.Property(e => e.Role)
               .HasColumnName("Rol")
               .HasMaxLength(15)
               .IsRequired()
               .HasConversion(
                    x => x.ToString(),
                    x => (RoleType)Enum.Parse(typeof(RoleType), x)
                );

            builder.Property(e => e.IdUsuario)
                .HasColumnName("id_usuario");

            builder.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Securitys)
                .HasForeignKey(d => d.IdUsuario)
                .HasConstraintName("FK_seguridad_usuario");
        }
    }
}