using System.ComponentModel.DataAnnotations;

namespace PicPay.Domains.Lojas
{
    public record LojaDTO(
        Guid UsuarioId,
        [Required(AllowEmptyStrings = false)]
        [MinLength(3, ErrorMessage = "Nome deve ser maior que 3")]
        string Nome
    )
    {
    }
}
