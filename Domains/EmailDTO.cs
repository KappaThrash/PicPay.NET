using System.ComponentModel.DataAnnotations;

namespace PicPay.Domains
{
    public record EmailDTO
    {

        [StringLength(70)]
        public required string PayerNome {  get; set; }
        [StringLength(70)]
        public required string PayeeNome { get; set; }

        [EmailAddress]
        public required string PayerEmail { get; set; }

        [EmailAddress]
        public required string PayeeEmail { get; set; }
        
        public required Decimal TransactionValue { get; set; }
        
        public DateTimeOffset TransactionTime { get; set; } = DateTimeOffset.Now;
    }
}
