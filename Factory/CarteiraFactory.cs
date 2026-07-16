using PicPay.Domains.Carteiras;

namespace PicPay.Factory
{
    public class CarteiraFactory
    {
        public static Carteira CarteiraBalance100() => new Carteira
        (
            100m,
            Guid.CreateVersion7()
        );

    }
}
