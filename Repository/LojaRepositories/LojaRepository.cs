using Microsoft.EntityFrameworkCore;
using PicPay.Domains.Lojas;
using PicPay.Repository.DataContext;
using PicPay.Repository.Template;

namespace PicPay.Repository.LojaRepositories
{
    public class LojaRepository : BasicRepository<Loja>, ILojaRepository
    {
        public LojaRepository(PicPayDbContext context) : base(context) {}

        public async Task<Loja> GetByUserId(Guid id)
        {
           return await _dbSet.FirstOrDefaultAsync(x => x.UsuarioId == id) 
                ?? throw new KeyNotFoundException("Loja não encontrada apartir do UsuarioId: " + id);
        }
    }
}
