using PicPay.Domains.Carteiras;

namespace PicPay.Domains.Transacoes
{
    public class Transacao
    {
        public Guid Id { get; set; }
        public Decimal Valor { get; set; }
        public Guid CarteiraPayerId { get; set; }
        public Guid CarteiraPayeeId { get; set; }

        public Carteira? CarteiraPayer { get; set; }
        public Carteira? CarteiraPayee { get; set; }

        protected Transacao() { }

        public Transacao(decimal valor, Guid carteiraPayerId, Guid carteiraPayeeId)
        {
            Valor = valor;
            CarteiraPayerId = carteiraPayerId;
            CarteiraPayeeId = carteiraPayeeId;
        }
    }
}
