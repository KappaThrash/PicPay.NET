using PicPay.Domains;

namespace PicPay.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> FindByIdAsync(Guid id);
        Task<Usuario?> FindByIdDisplayAsync(Guid id);
        Task<List<Usuario>> GetAll();
        Task<Usuario> SaveAsync(Usuario usuario);
        Task DeleteById(Guid id);
    }
}
