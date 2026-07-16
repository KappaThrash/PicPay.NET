using PicPay.Domains.Carteiras;

namespace PicPay.Domains.Transacoes
{
    public record TransacaoDTO
    {
        public Guid? Id { get; set; }
        public Decimal Valor { get; set; }
        public required Guid CarteiraPayerId { get; set; }
        public required Guid CarteiraPayeeId { get; set; }
        public Carteira? CarteiraPayer { get; set; }
        public Carteira? CarteiraPayee { get; set; }
    }
}
