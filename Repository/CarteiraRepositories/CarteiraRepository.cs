using Microsoft.EntityFrameworkCore;
using PicPay.Domains.Carteiras;
using PicPay.Repository.DataContext;

namespace PicPay.Repository.CarteiraRepositories
{
    public class CarteiraRepository(PicPayDbContext _DataBase) : ICarteiraRepository
    {
        public async Task DeleteAsync(Guid carteiraId)
        {
            await _DataBase.Carteiras.Where(x => x.Id == carteiraId).ExecuteDeleteAsync();
        }

        public async Task<Carteira?> FindByIdAsync(Guid id)
        {
            return await _DataBase.Carteiras.FindAsync(id);
        }

        public async Task<Carteira?> FindByIdWithUserAsync(Guid id)
        {
            return await _DataBase.Carteiras
                .Include(x => x.Usuario)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Carteira?> FindByUserIdAsync(Guid userId)
        {
            return await _DataBase.Carteiras.FirstOrDefaultAsync(x => x.UsuarioID == userId);
        }

        public async Task<Carteira> SaveCarteira(Carteira carteira)
        {
            _DataBase.Carteiras.Add(carteira);
            await _DataBase.SaveChangesAsync();
            return carteira;
        }
    }
}
