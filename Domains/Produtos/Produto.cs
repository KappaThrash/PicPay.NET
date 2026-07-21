using PicPay.Domains.Lojas;
using PicPay.Domains.Produtos.Enums;
using PicPay.Domains.Template;
using PicPay.Domains.Utils;

namespace PicPay.Domains.Produtos
{
    public class Produto : EntidadeBase
    {
        public Guid LojaId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public CategoriaProduto Categoria { get; set; } = CategoriaProduto.UNDEFINED;
        public Decimal Preco { get; set; }
        public Decimal Desconto { get; set; } = Decimal.Zero;
        public Imagem? Imagem { get; set; }
        public Loja? Loja { get; set; }

        public Produto() { }

        public Produto(string nome, string descricao, CategoriaProduto categoria, decimal preco, decimal desconto)
        {
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Descricao = descricao ?? throw new ArgumentNullException(nameof(descricao));
            Categoria = categoria;
            Preco = preco;
            Desconto = desconto;
        }
    }
}
