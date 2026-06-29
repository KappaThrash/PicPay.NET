namespace PicPay.Domains
{
    public class Imagem
    {
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public string ContentType { get; set; } = string.Empty;
        public string NomeImagem { get; set; } = string.Empty;
    }
}
