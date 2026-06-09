using Microsoft.EntityFrameworkCore.Metadata;
using PicPay.Domains;
using PicPay.Exceptions;
using PicPay.Repository;
using PicPay.Repository.DataContext;

namespace PicPay.Services
{
    public class TransacaoService(ITransacaoRepository _transacaoRepository, 
        ICarteiraRepository _carteiraRepository, PicPayDbContext _DataBase) : ITransacaoService
    {
        public async Task<TransacaoDTO> Processar(TransacaoDTO transacaoDTO)
        {
            var carteiraPayer = await _carteiraRepository.FindByIdWithUserAsync(transacaoDTO.CarteiraPayerId) ?? throw new KeyNotFoundException();

            var carteiraPayee = await _carteiraRepository.FindByIdAsync(transacaoDTO.CarteiraPayeeId) ?? throw new KeyNotFoundException();

            var UsuarioPayer = carteiraPayer.Usuario ?? throw new KeyNotFoundException();

            if (UsuarioPayer.IsLojista())
            {
                throw new BusinessException("Pagador não pode ser Lojista");
            }

            using var transaction = await _DataBase.Database.BeginTransactionAsync();

            try
            {
                carteiraPayer.Debitar(transacaoDTO.Valor);
                carteiraPayee.Creditar(transacaoDTO.Valor);

                var entity = await _transacaoRepository.Create(new Transacao(transacaoDTO.Valor, carteiraPayer.Id, carteiraPayee.Id));
                await _DataBase.SaveChangesAsync();

                await transaction.CommitAsync();

                transacaoDTO.Id = entity.Id;

                return transacaoDTO;
            }
            catch (Exception) {

                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ICollection<TransacaoDTO>> FindByAnyId(Guid id)
        {
            var transacao = await _transacaoRepository.FindByAnyId(id);
            return transacao.Select(e => new TransacaoDTO
            {
                Id = e.Id,
                CarteiraPayeeId = e.CarteiraPayeeId,
                CarteiraPayerId = e.CarteiraPayerId
            }).ToList();
        }

        public async Task<TransacaoDTO?> FindById(Guid id)
        {
            var transacao = await _transacaoRepository.FindById(id);

            if (transacao == null)
            {
                throw new KeyNotFoundException(nameof(transacao));
            }

            return new TransacaoDTO
            {
                Id = transacao.Id,
                CarteiraPayerId = transacao.CarteiraPayerId,
                CarteiraPayeeId = transacao.CarteiraPayeeId
            };
        }

        public async Task<ICollection<TransacaoDTO>> FindByPayeeId(Guid id)
        {
            var transacao = await _transacaoRepository.FindByPayeeId(id);
            return transacao.Select(e => new TransacaoDTO
            {
                Id = e.Id,
                CarteiraPayeeId = e.CarteiraPayeeId,
                CarteiraPayerId = e.CarteiraPayerId
            }).ToList();
        }

        public async Task<ICollection<TransacaoDTO>> FindByPayerId(Guid id)
        {
            var transacao = await _transacaoRepository.FindByPayerId(id);
            return transacao.Select(e => new TransacaoDTO
            {
                Id = e.Id,
                Valor = e.Valor ,
                CarteiraPayeeId = e.CarteiraPayeeId,
                CarteiraPayerId = e.CarteiraPayerId
            }).ToList();
        }
    }
}
