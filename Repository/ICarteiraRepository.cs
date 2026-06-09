using PicPay.Domains;

namespace PicPay.Repository
{
    public interface ICarteiraRepository
    {
        public Task<Carteira?> FindByIdAsync(Guid id);
        public Task<Carteira?> FindByIdWithUserAsync(Guid id);
        public Task<Carteira?> FindByUserIdAsync(Guid userId);
        public Task<Carteira> SaveCarteira(Carteira carteira);
        public Task DeleteAsync(Guid carteiraId);
    }
}
