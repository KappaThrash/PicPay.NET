using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PicPay.Domains.Lojas;
using PicPay.Domains.Models.Template;

namespace PicPay.Domains.Models
{
    public class LojaConfiguration : EntidadeBaseConfiguration<Loja>
    {
        public override void Configure(EntityTypeBuilder<Loja> builder)
        {
            base.Configure(builder);

            builder.Property(x => x.UsuarioId)
                .IsRequired();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(x => x.Nome)
                .IsUnique();

            builder.OwnsOne(x => x.Imagem, img =>
            {
                img.Property(i => i.Bytes)
                    .HasColumnName("ImagemBytes");

                img.Property(i => i.NomeImagem)
                    .HasColumnName("ImagemNome")
                    .HasMaxLength(100);
            });

            builder.HasMany(x => x.Produtos)
                .WithOne(y => y.Loja)
                .HasForeignKey(y => y.LojaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
