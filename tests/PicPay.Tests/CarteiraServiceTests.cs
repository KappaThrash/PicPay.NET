using Moq;
using PicPay.Domains;
using PicPay.Exceptions;
using PicPay.Repository.CarteiraRepositories;
using PicPay.Repository.UsuarioRepositories;
using PicPay.Services.CarteiraServices;
using Xunit;

namespace PicPay.Tests
{
    public class CarteiraServiceTests
    {
        private readonly CarteiraService _service;
        private readonly Mock<ICarteiraRepository> _carteiraRepositoryMock;
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock;

        public CarteiraServiceTests()
        {
            _carteiraRepositoryMock = new Mock<ICarteiraRepository>();
            _usuarioRepositoryMock = new Mock<IUsuarioRepository>();
            _service = new CarteiraService(_carteiraRepositoryMock.Object, _usuarioRepositoryMock.Object);
        }

        [Fact]
        public async Task DeleteCarteira_ShouldCallRepositoryDelete()
        {
            // Arrange
            var id = Guid.NewGuid();

            // Act
            await _service.DeleteCarteira(id);

            // Assert
            _carteiraRepositoryMock.Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Fact]
        public async Task FindByIdAsync_WhenExists_ReturnsCarteira()
        {
            // Arrange
            var id = Guid.NewGuid();
            var carteira = new Carteira(100m, Guid.NewGuid()) { Id = id };
            _carteiraRepositoryMock.Setup(r => r.FindByIdAsync(id)).ReturnsAsync(carteira);

            // Act
            var result = await _service.FindByIdAsync(id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(id, result!.Id);
        }

        [Fact]
        public async Task FindByIdAsync_WhenNotExists_ThrowsUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            _carteiraRepositoryMock.Setup(r => r.FindByIdAsync(id)).ReturnsAsync((Carteira?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _service.FindByIdAsync(id));
        }

        [Fact]
        public async Task FindByUserIdAsync_WhenExists_ReturnsCarteira()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var carteira = new Carteira(100m, userId);
            _carteiraRepositoryMock.Setup(r => r.FindByUserIdAsync(userId)).ReturnsAsync(carteira);

            // Act
            var result = await _service.FindByUserIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result!.UsuarioID);
        }

        [Fact]
        public async Task FindByUserIdAsync_WhenNotExists_ThrowsUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _carteiraRepositoryMock.Setup(r => r.FindByUserIdAsync(userId)).ReturnsAsync((Carteira?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _service.FindByUserIdAsync(userId));
        }

        [Fact]
        public async Task CreateCarteira_WhenUserExists_ReturnsSavedCarteira()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var usuario = new Usuario("Nome", "email@ex.com", TipoEnum.USUARIO, "123", DateOnly.FromDateTime(DateTime.UtcNow), "senha") { Id = userId };
            var expectedCarteira = new Carteira(0m, userId);

            _usuarioRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync(usuario);
            _carteiraRepositoryMock.Setup(r => r.SaveCarteira(It.IsAny<Carteira>())).ReturnsAsync(expectedCarteira);

            // Act
            var result = await _service.CreateCarteira(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UsuarioID);
            Assert.Equal(0m, result.Balance);
            _carteiraRepositoryMock.Verify(r => r.SaveCarteira(It.Is<Carteira>(c => c.UsuarioID == userId && c.Balance == 0m)), Times.Once);
        }

        [Fact]
        public async Task CreateCarteira_WhenUserNotExists_ThrowsUserNotFoundException()
        {
            // Arrange
            var userId = Guid.NewGuid();
            _usuarioRepositoryMock.Setup(r => r.FindByIdAsync(userId)).ReturnsAsync((Usuario?)null);

            // Act & Assert
            await Assert.ThrowsAsync<UserNotFoundException>(() => _service.CreateCarteira(userId));
        }
    }
}
