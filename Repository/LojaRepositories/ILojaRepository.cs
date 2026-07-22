using PicPay.Domains.Lojas;
using PicPay.Repository.Template;

namespace PicPay.Repository.LojaRepositories
{
    public interface ILojaRepository : IBasicRepository<Loja>
    {
        public Task<Loja> GetByUserId(Guid id);
    }
}
