using PicPay.Domains.Template;
using PicPay.Domains.Utils;

namespace PicPay.Domains.Produtos
{
    public class Produto : EntidadeBase
    {
        public Guid LojaId { get; set; }
        public Decimal Preco { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Imagem? Imagem { get; set; }
    }
}
