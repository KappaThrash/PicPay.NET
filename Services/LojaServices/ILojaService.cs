using PicPay.Domains.Lojas;

namespace PicPay.Services.LojaServices
{
    public interface ILojaService
    {
        public Task<Loja> CreateLojaAsync(LojaDTO lojaDTO);
        public Task<Loja> GetLojaByIdAsync(Guid id);
    }
}
