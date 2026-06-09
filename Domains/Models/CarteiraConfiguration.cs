using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PicPay.Domains.Models
{
    public class CarteiraConfiguration : IEntityTypeConfiguration<Carteira>
    {
        public void Configure(EntityTypeBuilder<Carteira> builder)
        {
            builder.ToTable("Carteiras");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Balance).HasPrecision(18,2).IsRequired();
        }
    }
}
