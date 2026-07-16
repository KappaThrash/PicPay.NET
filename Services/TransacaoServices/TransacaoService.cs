using Microsoft.EntityFrameworkCore.Metadata;
using PicPay.Domains;
using PicPay.Domains.Transacoes;
using PicPay.Exceptions;
using PicPay.Repository.CarteiraRepositories;
using PicPay.Repository.DataContext;
using PicPay.Repository.TransacaoRepositories;
using PicPay.Services.Notification;

namespace PicPay.Services.TransacaoServices
{
    /// <summary>
    /// Service responsible for managing money transfer transactions between wallets,
    /// enforcing business validations, database transactions, rollback handling, and notification publishing.
    /// </summary>
    public class TransacaoService(ITransacaoRepository _transacaoRepository, 
        ICarteiraRepository _carteiraRepository, PicPayDbContext _DataBase,
        INotificationSender _notificationSender) : ITransacaoService
    {
        /// <summary>
        /// Processes a financial transaction between two wallets, validating payer type and funds.
        /// Performs the transaction atomically, rolling back if database operations fail, and sends an email notification.
        /// </summary>
        /// <param name="transacaoDTO">The transfer request details (amount, payer wallet, payee wallet).</param>
        /// <returns>The processed transaction detail containing its generated ID.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if either wallet or their corresponding user accounts do not exist.</exception>
        /// <exception cref="BusinessException">Thrown if the payer is a merchant (Lojista) or has insufficient wallet balance.</exception>
        public async Task<TransacaoDTO> Processar(TransacaoDTO transacaoDTO)
        {
            var carteiraPayer = await _carteiraRepository.FindByIdWithUserAsync(transacaoDTO.CarteiraPayerId) ?? throw new KeyNotFoundException();

            var carteiraPayee = await _carteiraRepository.FindByIdWithUserAsync(transacaoDTO.CarteiraPayeeId) ?? throw new KeyNotFoundException();

            var UsuarioPayer = carteiraPayer.Usuario ?? throw new KeyNotFoundException();
            var UsuarioPayee = carteiraPayee.Usuario ?? throw new KeyNotFoundException();

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

                await _notificationSender.PublishEmail(
                    new EmailDTO
                    {
                        PayerNome = UsuarioPayer.Nome,
                        PayeeNome = UsuarioPayee.Nome,
                        PayerEmail = UsuarioPayer.Email,
                        PayeeEmail = UsuarioPayee.Email,
                        TransactionValue = transacaoDTO.Valor
                    }
                );

                return transacaoDTO;
            }
            catch (Exception) {

                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <summary>
        /// Finds all transactions where the specified ID is either the payer or payee wallet.
        /// </summary>
        /// <param name="id">The wallet ID to search for.</param>
        /// <returns>A collection of matching transaction DTOs.</returns>
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

        /// <summary>
        /// Finds a specific transaction by its unique identifier.
        /// </summary>
        /// <param name="id">The transaction ID.</param>
        /// <returns>The transaction details DTO, or throws KeyNotFoundException if not found.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no transaction with the specified ID exists.</exception>
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

        /// <summary>
        /// Finds all transactions where the specified wallet ID is the payee (receiver).
        /// </summary>
        /// <param name="id">The payee wallet ID.</param>
        /// <returns>A collection of transactions received by the payee.</returns>
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

        /// <summary>
        /// Finds all transactions where the specified wallet ID is the payer (sender).
        /// </summary>
        /// <param name="id">The payer wallet ID.</param>
        /// <returns>A collection of transactions sent by the payer.</returns>
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
