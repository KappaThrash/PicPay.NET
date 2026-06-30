using PicPay.Domains;

namespace PicPay.Services.CarteiraServices
{
    public interface ICarteiraService
    {
        public Task<Carteira?> FindByIdAsync(Guid id);
        public Task<Carteira?> FindByUserIdAsync(Guid userId);
        public Task<Carteira> CreateCarteira(Guid userId);

        public Task DeleteCarteira(Guid id);
    }
}
