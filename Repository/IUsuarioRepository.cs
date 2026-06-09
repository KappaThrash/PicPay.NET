using PicPay.Domains;

namespace PicPay.Repository
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> FindByIdAsync(Guid id);
        Task<List<Usuario>> GetAll();
        Task<Usuario> SaveAsync(Usuario usuario);
        Task<Usuario> DeleteById(Guid id);
    }
}
