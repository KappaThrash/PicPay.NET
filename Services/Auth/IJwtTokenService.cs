using PicPay.Domains;

namespace PicPay.Services.Auth
{
    public interface IJwtTokenService
    {
        AuthTokenDTO GenerateToken(Usuario usuario, IEnumerable<string>? roles = null);
    }
}
