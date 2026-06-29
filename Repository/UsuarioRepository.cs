using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using PicPay.Domains;
using PicPay.Repository.DataContext;

namespace PicPay.Repository
{
    public class UsuarioRepository(PicPayDbContext _DataBase) : IUsuarioRepository
    {
        public Task<Usuario> DeleteById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Usuario?> FindByIdAsync(Guid id) => await _DataBase.Usuarios.FindAsync(id);

        public async Task<Usuario?> FindByEmailAsync(string email) =>
            await _DataBase.Usuarios.FirstOrDefaultAsync(usuario => usuario.Email == email);

        public async Task<List<Usuario>> GetAll() => await _DataBase.Usuarios.ToListAsync();

        public async Task<Usuario> SaveAsync(Usuario usuario)
        {
            _DataBase.Usuarios.Add(usuario);
            await _DataBase.SaveChangesAsync();
            return usuario;
        }
    }
}
