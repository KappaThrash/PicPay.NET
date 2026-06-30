using Microsoft.EntityFrameworkCore;
using PicPay.Domains;
using PicPay.Exceptions;
using PicPay.Repository.DataContext;

namespace PicPay.Repository.UsuarioRepositories
{
    public class UsuarioRepository(PicPayDbContext _DataBase) : IUsuarioRepository
    {
        public async Task DeleteById(Guid id)
        {
            var usuario = await _DataBase.Usuarios.FirstOrDefaultAsync(x => x.Id == id);

            if (usuario == null)
            {
                throw new UserNotFoundException($"Usuario com id {id}, não encontrado");
            }

            _DataBase.Usuarios.Remove(usuario);
            await _DataBase.SaveChangesAsync();
        }

        public async Task<Usuario?> FindByIdAsync(Guid id) => await _DataBase.Usuarios.FindAsync(id);

        public async Task<Usuario?> FindByIdDisplayAsync(Guid id) => await _DataBase.Usuarios.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Usuario>> GetAll() => await _DataBase.Usuarios.AsNoTracking().ToListAsync();

        public async Task<Usuario> SaveAsync(Usuario usuario)
        {
            _DataBase.Usuarios.Add(usuario);
            await _DataBase.SaveChangesAsync();
            return usuario;
        }
    }
}
