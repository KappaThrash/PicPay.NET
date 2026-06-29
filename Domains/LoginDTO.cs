using System.ComponentModel.DataAnnotations;

namespace PicPay.Domains
{
    public record LoginDTO
    {
        [EmailAddress(ErrorMessage = "Email Invalido")]
        public required string Email { get; set; }

        [Required(AllowEmptyStrings = false)]
        public required string Senha { get; set; }
    }
}
