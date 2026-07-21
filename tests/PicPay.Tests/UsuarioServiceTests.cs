using Microsoft.AspNetCore.Http;
using Moq;
using PicPay.Domains.Utils;
using PicPay.Domains.Usuarios;
using PicPay.Factory;
using PicPay.Repository.UsuarioRepositories;
using PicPay.Services.UsuarioServices;


namespace PicPay.Tests
{
    public class UsuarioServiceTests
    {
        private readonly UsuarioService _service;
        private readonly Mock<IUsuarioRepository> usuarioRepository;
        //private readonly IFormFile file;

        public UsuarioServiceTests()
        {
           usuarioRepository = new Mock<IUsuarioRepository>();

           _service = new UsuarioService(usuarioRepository.Object);

            var usuario = UsuarioFactory.UsuarioLojista();

            //using var stream = new MemoryStream();
            //file = new FormFile(stream, 0,stream.Length,"formtest","filetest");
        }

        [Fact]
        public async Task FindUsuarioAsync_WhenExists_ReturnsUsuario()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expected = new Usuario("Nome", "email@ex.com", TipoEnum.USUARIO, "123", DateOnly.FromDateTime(DateTime.UtcNow), "senha");
            expected.Id = id;
            usuarioRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(expected);

            // Act
            var result = await _service.FindUsuarioAsync(id);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(id, result!.Id);
        }

        [Xunit.FactAttribute]
        public async Task FindUsuarioAsync_WhenNotExists_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            usuarioRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Usuario?)null);

            // Act
            var result = await _service.FindUsuarioAsync(id);

            // Assert
            Xunit.Assert.Null(result);
        }

        [Xunit.FactAttribute]
        public async Task SaveUsuarioAsync_WithValidDto_CallsRepositoryAndReturnsUsuario()
        {
            // Arrange
            var dto = new UsuarioDTO
            {
                Nome = "Nome",
                Email = "a@b.com",
                Tipo = TipoEnum.LOJISTA,
                Cpf = "0000000",
                DataNascimento = DateOnly.FromDateTime(DateTime.UtcNow),
                Senha = "senha"
            };

            usuarioRepository.Setup(r => r.SaveAsync(It.IsAny<Usuario>()))
                .ReturnsAsync((Usuario u) => { u.Id = Guid.NewGuid(); return u; });

            // Act
            var result = await _service.SaveUsuarioAsync(dto);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(dto.Nome, result.Nome);
            Xunit.Assert.Equal(dto.Email, result.Email);
            Xunit.Assert.Equal(dto.Tipo, result.Tipo);
            Xunit.Assert.Equal(dto.Cpf, result.Cpf);
            Xunit.Assert.Equal(dto.DataNascimento, result.DataNascimento);
        }

        [Xunit.FactAttribute]
        public async Task SaveUsuarioAsync_NullDto_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Xunit.Assert.ThrowsAsync<ArgumentNullException>(async () => await _service.SaveUsuarioAsync(null!));
        }

        [Xunit.FactAttribute]
        public async Task SaveUsuarioImagemAsync_WithValidFile_SavesImageAndReturnsUsuario()
        {
            // Arrange
            var id = Guid.NewGuid();
            var imageData = new byte[] { 10, 20, 30 };

            var fileMock = new Mock<IFormFile>();
            fileMock.SetupGet(f => f.ContentType).Returns("image/png");
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<System.Threading.CancellationToken>())).Returns((Stream s, System.Threading.CancellationToken _) => s.WriteAsync(imageData, 0, imageData.Length));

            var usuario = new Usuario("Nome", "email@ex.com", TipoEnum.USUARIO, "123", DateOnly.FromDateTime(DateTime.UtcNow), "senha")
            {
                Id = id,
                Imagem = new Imagem()
            };

            usuarioRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync(usuario);
            usuarioRepository.Setup(r => r.SaveAsync(It.IsAny<Usuario>())).ReturnsAsync((Usuario u) => u);

            // Act
            var result = await _service.SaveUsuarioImagemAsync(fileMock.Object, id);

            // Assert
            Xunit.Assert.NotNull(result);
            Xunit.Assert.Equal(imageData, result!.Imagem!.Bytes);
            Xunit.Assert.Equal("image/png", result.Imagem.ContentType);
            Xunit.Assert.Equal($"{id}UsuarioImagem", result.Imagem.NomeImagem);

            usuarioRepository.Verify(r => r.SaveAsync(It.Is<Usuario>(u => u.Imagem != null && u.Imagem.Bytes.SequenceEqual(imageData))), Times.Once);
        }

        [Xunit.FactAttribute]
        public async Task SaveUsuarioImagemAsync_UserNotFound_ThrowsUserNotFoundException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), It.IsAny<System.Threading.CancellationToken>())).Returns(Task.CompletedTask);

            usuarioRepository.Setup(r => r.GetByIdAsync(id)).ReturnsAsync((Usuario?)null);

            // Act & Assert
            await Xunit.Assert.ThrowsAsync<PicPay.Exceptions.UserNotFoundException>(async () => await _service.SaveUsuarioImagemAsync(fileMock.Object, id));
        }

    }
}
