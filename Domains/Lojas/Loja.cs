using PicPay.Domains.Template;
using PicPay.Domains.Utils;
using PicPay.Domains.Produtos;

namespace PicPay.Domains.Lojas
{
    public class Loja : EntidadeBase
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public Imagem? Imagem { get; set; }
        public ICollection<Guid> Membros { get; set; } = new List<Guid>();
        public ICollection<Produto>? Produtos { get; set; }
    }
}
