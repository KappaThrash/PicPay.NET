using PicPay.Domains;

namespace PicPay.Repository.TransacaoRepositories
{
    public interface ITransacaoRepository
    {
        public Task<Transacao?> FindById(Guid id);
        public Task<ICollection<Transacao>> FindByPayerId(Guid id);
        public Task<ICollection<Transacao>> FindByPayeeId(Guid id);
        public Task<ICollection<Transacao>> FindByAnyId(Guid id);
        public Task<Transacao> Create(Transacao transacao);
    }
}
