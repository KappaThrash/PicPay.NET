using PicPay.Domains;

namespace PicPay.Services.UsuarioServices
{
    public interface IUsuarioService
    {
        /// <summary>
        /// Encontrar Usuario De acordo com o Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<Usuario?> FindUsuarioAsync(Guid id);
        public Task<Usuario> SaveUsuarioAsync(UsuarioDTO usuarioDTO);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="UserNotFoundException"></exception>
        public Task<Usuario?> SaveUsuarioImagemAsync(IFormFile file, Guid id);
        public Task DeleteUsuarioAsync(Guid id);

    }
}
