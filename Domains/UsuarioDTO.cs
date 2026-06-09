
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace PicPay.Domains
{
    public record UsuarioDTO
    {
        [Required(AllowEmptyStrings = false)]
        [MinLength(4, ErrorMessage = "Nome deve ser maior que 4")]
        public required string Nome { get; set; }
        [EmailAddress(ErrorMessage = "Email Invalido")]
        public required string Email { get; set; }
        public required TipoEnum Tipo { get; set; }
        public required string Cpf { get; set; }
        public required DateOnly DataNascimento { get; set; }
        [Required(AllowEmptyStrings = false)]
        [MinLength(4, ErrorMessage = "Senha tem que ser maior do que 4")]
        public required string Senha { get; set; }
    }
}
