using PicPay.Domains;
using PicPay.Repository.Template;

namespace PicPay.Repository.UsuarioRepositories
{
    public interface IUsuarioRepository : IBasicRepository<Usuario>
    {
        Task<Usuario?> FindByIdDisplayAsync(Guid id);
        Task<Usuario> SaveAsync(Usuario usuario);
    }
}
