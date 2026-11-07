using Domain.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configuration;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.HasKey(u => u.IdUsuario);

        builder.Property(u => u.NomeUsuario)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.EnderecoEmailUsuario)
            .IsRequired()
            .HasMaxLength(254);

        builder.Property(u => u.SenhaUsuario)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(u => u.ContaDeletada)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(u => u.EnderecoEmailUsuario)
            .IsUnique();
    }
}
