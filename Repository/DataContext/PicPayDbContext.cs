using Microsoft.EntityFrameworkCore;
using PicPay.Domains;
using System.Reflection;

namespace PicPay.Repository.DataContext
{
    public class PicPayDbContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Carteira> Carteiras { get; set; }
        public DbSet<Transacao> Transacoes { get; set; }

        public PicPayDbContext(DbContextOptions<PicPayDbContext> options) : base(options)
        {
        }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Carteira).Assembly);
            }
    }
}
