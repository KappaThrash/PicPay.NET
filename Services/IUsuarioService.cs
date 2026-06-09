using PicPay.Domains;

namespace PicPay.Services
{
    public interface IUsuarioService
    {
        public Task<Usuario?> FindUsuarioAsync(Guid id);
        public Task<Usuario> SaveUsuarioAsync(UsuarioDTO usuarioDTO);
    }
}
