namespace PicPay.Domains
{
    public record CarteiraDTO
    {
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public Guid UsuarioID { get; set; }

        public CarteiraDTO(Guid CarteiraId,decimal balance, Guid usuarioID)
        {
            Id = CarteiraId; 
            Balance = balance;
            UsuarioID = usuarioID;
        }
    }
}
