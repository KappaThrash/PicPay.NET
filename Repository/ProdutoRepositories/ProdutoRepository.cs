using PicPay.Repository.Template;
using PicPay.Domains.Produtos;
using Microsoft.EntityFrameworkCore;
using PicPay.Domains.Produtos.Enums;

namespace PicPay.Repository.ProdutoRepositories
{
    public class ProdutoRepository : BasicRepository<Produto>, IProdutoRepository
    {
        public ProdutoRepository(DbContext context) : base(context)
        {
        }

        public async Task<Produto> GetByNomeAndLojaIdAsync(string nome, Guid lojaId)
        {
            var usuario = await _dbSet.FirstOrDefaultAsync(p => p.Nome == nome && p.LojaId == lojaId);
            return usuario!;
        }
        public async Task<IEnumerable<Produto>> GetByCategoria(CategoriaProduto categoria, Guid lojaId)
        {
            var produtos = await _dbSet
                .Where(p => p.Categoria == categoria && p.LojaId == lojaId)
                .ToListAsync();
            return produtos;
        }
    }
}