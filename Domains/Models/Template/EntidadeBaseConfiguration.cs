using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PicPay.Domains.Template;

namespace PicPay.Domains.Models.Template
{
    public abstract class EntidadeBaseConfiguration<T> : IEntityTypeConfiguration<T> where T : EntidadeBase
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()")
                .IsRequired();

            builder.Property(x => x.Ativo)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(x => x.DataCriacao)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.DataAtualizacao).HasColumnType("datetime2");
        }
    }
}
