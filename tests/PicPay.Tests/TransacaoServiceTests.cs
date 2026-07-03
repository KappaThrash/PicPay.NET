using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using PicPay.Domains;
using PicPay.Exceptions;
using PicPay.Repository.CarteiraRepositories;
using PicPay.Repository.DataContext;
using PicPay.Repository.TransacaoRepositories;
using PicPay.Services.Notification;
using PicPay.Services.TransacaoServices;
using Xunit;

namespace PicPay.Tests
{
    public class TransacaoServiceTests
    {
        private readonly Mock<ITransacaoRepository> _transacaoRepositoryMock;
        private readonly Mock<ICarteiraRepository> _carteiraRepositoryMock;
        private readonly Mock<INotificationSender> _notificationSenderMock;

        public TransacaoServiceTests()
        {
            _transacaoRepositoryMock = new Mock<ITransacaoRepository>();
            _carteiraRepositoryMock = new Mock<ICarteiraRepository>();
            _notificationSenderMock = new Mock<INotificationSender>();
        }

        private PicPayDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<PicPayDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .Options;
            return new PicPayDbContext(options);
        }

        [Fact]
        public async Task Processar_PayerWalletNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var dto = new TransacaoDTO
            {
                CarteiraPayerId = Guid.NewGuid(),
                CarteiraPayeeId = Guid.NewGuid(),
                Valor = 50m
            };

            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayerId))
                .ReturnsAsync((Carteira?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Processar(dto));
        }

        [Fact]
        public async Task Processar_PayeeWalletNotFound_ThrowsKeyNotFoundException()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var dto = new TransacaoDTO
            {
                CarteiraPayerId = Guid.NewGuid(),
                CarteiraPayeeId = Guid.NewGuid(),
                Valor = 50m
            };

            var payerWallet = new Carteira(100m, Guid.NewGuid()) { Id = dto.CarteiraPayerId };

            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayerId))
                .ReturnsAsync(payerWallet);
            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayeeId))
                .ReturnsAsync((Carteira?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Processar(dto));
        }

        [Fact]
        public async Task Processar_PayerOrPayeeUserIsNull_ThrowsKeyNotFoundException()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var dto = new TransacaoDTO
            {
                CarteiraPayerId = Guid.NewGuid(),
                CarteiraPayeeId = Guid.NewGuid(),
                Valor = 50m
            };

            var payerWallet = new Carteira(100m, Guid.NewGuid()) { Id = dto.CarteiraPayerId, Usuario = null };
            var payeeWallet = new Carteira(100m, Guid.NewGuid()) { Id = dto.CarteiraPayeeId, Usuario = null };

            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayerId))
                .ReturnsAsync(payerWallet);
            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayeeId))
                .ReturnsAsync(payeeWallet);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.Processar(dto));
        }

        [Fact]
        public async Task Processar_PayerIsLojista_ThrowsBusinessException()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var dto = new TransacaoDTO
            {
                CarteiraPayerId = Guid.NewGuid(),
                CarteiraPayeeId = Guid.NewGuid(),
                Valor = 50m
            };

            var payerUser = new Usuario("Lojista", "l@l.com", TipoEnum.LOJISTA, "123", DateOnly.FromDateTime(DateTime.UtcNow), "pwd");
            var payeeUser = new Usuario("Usuario", "u@u.com", TipoEnum.USUARIO, "456", DateOnly.FromDateTime(DateTime.UtcNow), "pwd");

            var payerWallet = new Carteira(100m, payerUser.Id) { Id = dto.CarteiraPayerId, Usuario = payerUser };
            var payeeWallet = new Carteira(100m, payeeUser.Id) { Id = dto.CarteiraPayeeId, Usuario = payeeUser };

            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayerId))
                .ReturnsAsync(payerWallet);
            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayeeId))
                .ReturnsAsync(payeeWallet);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BusinessException>(() => service.Processar(dto));
        }

        [Fact]
        public async Task Processar_ValidTransaction_PerformsTransactionAndSendsNotification()
        {
            // Arrange
            using var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var dto = new TransacaoDTO
            {
                CarteiraPayerId = Guid.NewGuid(),
                CarteiraPayeeId = Guid.NewGuid(),
                Valor = 40m
            };

            var payerUser = new Usuario("Payer", "payer@email.com", TipoEnum.USUARIO, "123", DateOnly.FromDateTime(DateTime.UtcNow), "pwd");
            var payeeUser = new Usuario("Payee", "payee@email.com", TipoEnum.LOJISTA, "456", DateOnly.FromDateTime(DateTime.UtcNow), "pwd");

            var payerWallet = new Carteira(100m, payerUser.Id) { Id = dto.CarteiraPayerId, Usuario = payerUser };
            var payeeWallet = new Carteira(50m, payeeUser.Id) { Id = dto.CarteiraPayeeId, Usuario = payeeUser };

            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayerId))
                .ReturnsAsync(payerWallet);
            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayeeId))
                .ReturnsAsync(payeeWallet);

            var expectedTransaction = new Transacao(dto.Valor, payerWallet.Id, payeeWallet.Id) { Id = Guid.NewGuid() };
            _transacaoRepositoryMock.Setup(r => r.Create(It.IsAny<Transacao>()))
                .ReturnsAsync(expectedTransaction);

            // Act
            var result = await service.Processar(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedTransaction.Id, result.Id);
            Assert.Equal(60m, payerWallet.Balance); // 100 - 40
            Assert.Equal(90m, payeeWallet.Balance); // 50 + 40

            _transacaoRepositoryMock.Verify(r => r.Create(It.Is<Transacao>(t => t.Valor == dto.Valor && t.CarteiraPayerId == payerWallet.Id && t.CarteiraPayeeId == payeeWallet.Id)), Times.Once);
            _notificationSenderMock.Verify(n => n.PublishEmail(It.Is<EmailDTO>(e => 
                e.PayerNome == payerUser.Nome &&
                e.PayeeNome == payeeUser.Nome &&
                e.PayerEmail == payerUser.Email &&
                e.PayeeEmail == payeeUser.Email &&
                e.TransactionValue == dto.Valor
            )), Times.Once);
        }

        [Fact]
        public async Task Processar_InsufficientFunds_ThrowsBusinessExceptionAndRollsBack()
        {
            // Arrange
            using var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var dto = new TransacaoDTO
            {
                CarteiraPayerId = Guid.NewGuid(),
                CarteiraPayeeId = Guid.NewGuid(),
                Valor = 200m
            };

            var payerUser = new Usuario("Payer", "payer@email.com", TipoEnum.USUARIO, "123", DateOnly.FromDateTime(DateTime.UtcNow), "pwd");
            var payeeUser = new Usuario("Payee", "payee@email.com", TipoEnum.LOJISTA, "456", DateOnly.FromDateTime(DateTime.UtcNow), "pwd");

            var payerWallet = new Carteira(100m, payerUser.Id) { Id = dto.CarteiraPayerId, Usuario = payerUser };
            var payeeWallet = new Carteira(50m, payeeUser.Id) { Id = dto.CarteiraPayeeId, Usuario = payeeUser };

            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayerId))
                .ReturnsAsync(payerWallet);
            _carteiraRepositoryMock.Setup(r => r.FindByIdWithUserAsync(dto.CarteiraPayeeId))
                .ReturnsAsync(payeeWallet);

            // Act & Assert
            await Assert.ThrowsAsync<BusinessException>(() => service.Processar(dto));
            Assert.Equal(100m, payerWallet.Balance); // Unchanged
            Assert.Equal(50m, payeeWallet.Balance);   // Unchanged
            _transacaoRepositoryMock.Verify(r => r.Create(It.IsAny<Transacao>()), Times.Never);
        }

        [Fact]
        public async Task FindById_WhenExists_ReturnsTransacaoDTO()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var id = Guid.NewGuid();
            var transacao = new Transacao(100m, Guid.NewGuid(), Guid.NewGuid()) { Id = id };
            _transacaoRepositoryMock.Setup(r => r.FindById(id)).ReturnsAsync(transacao);

            // Act
            var result = await service.FindById(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
            Assert.Equal(transacao.CarteiraPayerId, result.CarteiraPayerId);
            Assert.Equal(transacao.CarteiraPayeeId, result.CarteiraPayeeId);
        }

        [Fact]
        public async Task FindById_WhenNotExists_ThrowsKeyNotFoundException()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var id = Guid.NewGuid();
            _transacaoRepositoryMock.Setup(r => r.FindById(id)).ReturnsAsync((Transacao?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.FindById(id));
        }

        [Fact]
        public async Task FindByAnyId_ReturnsMatchingTransactions()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var id = Guid.NewGuid();
            var transactions = new List<Transacao>
            {
                new Transacao(50m, id, Guid.NewGuid()) { Id = Guid.NewGuid() },
                new Transacao(30m, Guid.NewGuid(), id) { Id = Guid.NewGuid() }
            };
            _transacaoRepositoryMock.Setup(r => r.FindByAnyId(id)).ReturnsAsync(transactions);

            // Act
            var result = await service.FindByAnyId(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Id == transactions[0].Id);
            Assert.Contains(result, t => t.Id == transactions[1].Id);
        }

        [Fact]
        public async Task FindByPayeeId_ReturnsMatchingTransactions()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var payeeId = Guid.NewGuid();
            var transactions = new List<Transacao>
            {
                new Transacao(50m, Guid.NewGuid(), payeeId) { Id = Guid.NewGuid() }
            };
            _transacaoRepositoryMock.Setup(r => r.FindByPayeeId(payeeId)).ReturnsAsync(transactions);

            // Act
            var result = await service.FindByPayeeId(payeeId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(transactions[0].Id, result.First().Id);
        }

        [Fact]
        public async Task FindByPayerId_ReturnsMatchingTransactions()
        {
            // Arrange
            var dbContext = CreateDbContext();
            var service = new TransacaoService(
                _transacaoRepositoryMock.Object,
                _carteiraRepositoryMock.Object,
                dbContext,
                _notificationSenderMock.Object
            );

            var payerId = Guid.NewGuid();
            var transactions = new List<Transacao>
            {
                new Transacao(50m, payerId, Guid.NewGuid()) { Id = Guid.NewGuid() }
            };
            _transacaoRepositoryMock.Setup(r => r.FindByPayerId(payerId)).ReturnsAsync(transactions);

            // Act
            var result = await service.FindByPayerId(payerId);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(transactions[0].Id, result.First().Id);
            Assert.Equal(50m, result.First().Valor);
        }
    }
}
