using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PicPay.Domains.Models
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Nome).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Tipo).HasConversion<string>().IsRequired();
            builder.Property(x => x.Cpf).HasMaxLength(50).IsRequired();
            builder.Property(x => x.DataNascimento).IsRequired();
            builder.Property(x => x.Senha).HasMaxLength(50).IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
            builder.HasIndex(x => x.Cpf).IsUnique();

            builder.OwnsOne(x => x.Imagem, img =>
            {
                img.Property(i => i.Bytes).HasColumnType("VARBINARY(MAX)").HasColumnName("Bytes_Imagem");
                img.Property(i => i.ContentType).HasMaxLength(50).HasColumnName("ContentType_Imagem");
                img.Property(i => i.NomeImagem).HasMaxLength(50).HasColumnName("Nome_Imagem");

            }   );

            builder.HasOne(x => x.carteira).WithOne(x => x.Usuario).HasForeignKey<Carteira>(x => x.UsuarioID);
        }
    }
}
