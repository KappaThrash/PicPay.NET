using PicPay.Domains.Produtos;
using PicPay.Domains.Produtos.Enums;
using PicPay.Repository.Template;

namespace PicPay.Repository.ProdutoRepositories
{
    public interface IProdutoRepository : IBasicRepository<Produto>
    {
        public Task<Produto> GetByNomeAndLojaIdAsync(string nome, Guid lojaId);
        public Task<IEnumerable<Produto>> GetByCategoria(CategoriaProduto categoria, Guid lojaId);

    }
}
