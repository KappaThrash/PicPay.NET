using PicPay.Domains;

namespace PicPay.Tests;

public class UsuarioTests
{
    private readonly Usuario usuario;

    public UsuarioTests()
    {
        usuario = new Usuario(
            "Maria Clara",
            "maria@gmail.com",
            TipoEnum.LOJISTA,
            "1111111111",
            new DateOnly(2007, 4, 17),
            "ABC123"
        );
    }

    [Fact]
    public void IsLojistaTest()
    {
        Assert.True(usuario.IsLojista());
    }

    [Fact]
    public void CarteiraShouldBeNull()
    {
        Assert.Null(usuario.carteira);
    }
}
