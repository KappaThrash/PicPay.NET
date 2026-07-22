using PicPay.Domains.Lojas;
using PicPay.Exceptions;
using PicPay.Repository.LojaRepositories;
using PicPay.Repository.UsuarioRepositories;

namespace PicPay.Services.LojaServices
{
    public class LojaService(ILojaRepository lojaRepository,
        IUsuarioRepository usuarioRepository
        ) : ILojaService {
        public async Task<Loja> CreateLojaAsync(LojaDTO lojaDTO)
        {
            var usuario = await usuarioRepository.GetByIdAsync(lojaDTO.UsuarioId) ?? throw new Exception("Usuário não encontrado");

            if (!usuario.IsLojista())
            {
                throw new BusinessException("Apenas usuários lojistas podem criar lojas.");
            }

            Loja loja = new()
            {
                UsuarioId = lojaDTO.UsuarioId,
                Nome = lojaDTO.Nome
            };

            await lojaRepository.AddAsync(loja);
            await lojaRepository.SaveChangesAsync();

            return loja;
        }

        public async Task<Loja> GetLojaByIdAsync(Guid id)
        {
            var loja = await lojaRepository.GetByIdAsync(id) ?? throw new Exception("Loja não encontrada");
            return loja;

        }
    }
}
