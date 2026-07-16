namespace PicPay.Domains.Template
{
    public abstract class EntidadeBase
    {
        public Guid Id {  get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
