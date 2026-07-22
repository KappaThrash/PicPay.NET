using Microsoft.EntityFrameworkCore;
using PicPay.Domains.Carteiras;
using PicPay.Domains.Lojas;
using PicPay.Domains.Transacoes;
using PicPay.Domains.Usuarios;
using PicPay.Domains.Produtos;

namespace PicPay.Repository.DataContext
{
    public class PicPayDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Carteira> Carteiras { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }
        public DbSet<Loja> Lojas { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        public PicPayDbContext(DbContextOptions<PicPayDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Carteira).Assembly);
        }
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<Enum>().HaveConversion<String>();
            base.ConfigureConventions(configurationBuilder);
        }
    }
}
