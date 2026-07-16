using Microsoft.EntityFrameworkCore;
using PicPay.Domains.Transacoes;
using PicPay.Repository.DataContext;

namespace PicPay.Repository.TransacaoRepositories
{
    public class TransacaoRepository(PicPayDbContext _Database) : ITransacaoRepository
    {
        public async Task<Transacao> Create(Transacao transacao)
        {
            await _Database.Transacoes.AddAsync(transacao);
            await _Database.SaveChangesAsync();
            return transacao;
        }

        public async Task<Transacao?> FindById(Guid id)
        {
            return await _Database.Transacoes.FindAsync(id);
        }
        public async Task<ICollection<Transacao>> FindByAnyId(Guid id)
        {
            return await _Database.Transacoes
                .AsNoTracking()
                .Where(x => x.CarteiraPayerId == id || x.CarteiraPayeeId == id)
                .ToListAsync();
        }

        public async Task<ICollection<Transacao>> FindByPayeeId(Guid payeeId)
        {
            return await _Database.Transacoes
                .AsNoTracking()
                .Where(x => x.CarteiraPayeeId == payeeId)
                .ToListAsync();
        }

        public async Task<ICollection<Transacao>> FindByPayerId(Guid payerId)
        {
            return await _Database.Transacoes
                .AsNoTracking()
                .Where(x => x.CarteiraPayerId == payerId)
                .ToListAsync();
        }
    }
}
