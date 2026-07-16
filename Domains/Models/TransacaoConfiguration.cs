using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PicPay.Domains.Transacoes;

namespace PicPay.Domains.Models
{
    public class TransacaoConfiguration : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            builder.ToTable("Transacoes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Valor).HasPrecision(18, 2).IsRequired();

            builder.HasOne(x => x.CarteiraPayer)
                .WithMany().HasForeignKey(x => x.CarteiraPayerId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.CarteiraPayee)
                .WithMany().HasForeignKey(x => x.CarteiraPayeeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
