using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PicPay.Domains.Models.Template;
using PicPay.Domains.Produtos;

namespace PicPay.Domains.Models
{
    public class ProdutoConfiguration : EntidadeBaseConfiguration<Produto>
    {
        public override void Configure(EntityTypeBuilder<Produto> builder)
        {
            base.Configure(builder);

            builder.HasAlternateKey(p => new { p.Nome, p.LojaId });

            builder.Property(p => p.Nome).HasMaxLength(128).IsRequired();
            builder.Property(p => p.Descricao).HasMaxLength(500).IsRequired();

            builder.Property(p => p.Categoria).IsRequired();

            builder.Property(p => p.Preco).HasPrecision(18, 2).IsRequired();
            builder.Property(p => p.Desconto).HasPrecision(18, 2).IsRequired();

            builder.OwnsOne(x => x.Imagem, img =>
            {
                img.Property(i => i.Bytes).HasColumnType("VARBINARY(MAX)").HasColumnName("Bytes_Imagem");
                img.Property(i => i.ContentType).HasMaxLength(50).HasColumnName("ContentType_Imagem");
                img.Property(i => i.NomeImagem).HasMaxLength(50).HasColumnName("Nome_Imagem");

            });
        }
    }
}
