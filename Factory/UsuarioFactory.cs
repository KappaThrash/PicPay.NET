using PicPay.Domains.Usuarios;

namespace PicPay.Factory
{
    public sealed class UsuarioFactory
    {
        public static Usuario UsuarioLojista() => new Usuario(
            "Maria Clara",
            "maria@gmail.com",
            TipoEnum.LOJISTA,
            "1111111111",
            new DateOnly(2007, 4, 17),
            "ABC123"
        );

        public static Usuario UsuarioUsuario() => new Usuario(
            "Maria Clara",
            "maria@gmail.com",
            TipoEnum.USUARIO,
            "1111111111",
            new DateOnly(2007, 4, 17),
            "ABC123"
        );
    }
}
