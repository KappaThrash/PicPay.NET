using PicPay.Domains.Usuarios;
using PicPay.Factory;

namespace PicPay.Tests;

public class UsuarioTests
{
    private readonly Usuario usuario;

    public UsuarioTests()
    {
        usuario = UsuarioFactory.UsuarioLojista();
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
