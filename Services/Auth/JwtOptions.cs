namespace PicPay.Services.Auth
{
    public class JwtOptions
    {
        public const string SectionName = "Jwt";

        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string Secret { get; set; } = string.Empty;
        public int ExpirationMinutes { get; set; } = 60;
        public string RoleClaimType { get; set; } = System.Security.Claims.ClaimTypes.Role;
    }
}
