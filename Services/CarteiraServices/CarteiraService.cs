using PicPay.Domains;
using PicPay.Exceptions;
using PicPay.Repository.CarteiraRepositories;
using PicPay.Repository.UsuarioRepositories;

namespace PicPay.Services.CarteiraServices
{
    public class CarteiraService(ICarteiraRepository _carteiraRepository, IUsuarioRepository _usuarioRepository) : ICarteiraService
    {
        public async Task DeleteCarteira(Guid id)
        {
            await _carteiraRepository.DeleteAsync(id);
        }

        public async Task<Carteira?> FindByIdAsync(Guid id)
        {
            Carteira? carteira = await _carteiraRepository.FindByIdAsync(id) 
                ?? throw new UserNotFoundException("Usuario Não encontrado apartir do ID recebido");
            return carteira;
           
        }

        public async Task<Carteira?> FindByUserIdAsync(Guid userId)
        {
            Carteira? carteira = await _carteiraRepository.FindByUserIdAsync(userId) 
                ?? throw new UserNotFoundException("Usuario não encontrado apartir do UserID recebido");
            return carteira;
        }

        public async Task<Carteira> CreateCarteira(Guid userId)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(userId) 
                ?? throw new UserNotFoundException("Usuario não encontrado, carteira não pode ser criada.");

            Carteira carteira = new Carteira(0, userId);

            return await _carteiraRepository.SaveCarteira(carteira);
        }
    }
}
