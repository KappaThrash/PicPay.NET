namespace PicPay.Domains
{
    public record AuthTokenDTO
    {
        public required string AccessToken { get; set; }
        public required string TokenType { get; set; }
        public required DateTime ExpiresAt { get; set; }
    }
}
