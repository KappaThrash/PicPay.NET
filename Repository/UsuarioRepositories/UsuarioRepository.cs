using Microsoft.EntityFrameworkCore;
using PicPay.Domains;
using PicPay.Exceptions;
using PicPay.Repository.DataContext;
using PicPay.Repository.Template;

namespace PicPay.Repository.UsuarioRepositories
{
    public class UsuarioRepository : BasicRepository<Usuario>, IUsuarioRepository
    {

        public UsuarioRepository(PicPayDbContext dataBase) : base(dataBase) {}

        public async Task<Usuario?> FindByIdDisplayAsync(Guid id) => await _dbSet.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        public async Task<Usuario> SaveAsync(Usuario usuario)
        {
            _dbSet.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }
    }
}
