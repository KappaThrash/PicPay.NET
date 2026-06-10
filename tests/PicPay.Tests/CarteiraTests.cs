using PicPay.Domains;
using PicPay.Exceptions;
using PicPay.Factory;


namespace PicPay.Tests
{
    public class CarteiraTests
    {
        public readonly Carteira carteira;

        public CarteiraTests()
        {
            carteira = CarteiraFactory.CarteiraBalance100();
        }

        [Fact]
        public void TestDebitar()
        {
            carteira.Debitar(10);
            Assert.Equal(90, carteira.Balance);
        }

        [Fact]
        public void TestCreditar()
        {
            carteira.Creditar(10);
            Assert.Equal(110, carteira.Balance);
        }

        [Fact]
        public void CarteiraShouldThrowBusinessException_Debitar0()
        {
            Assert.Throws<BusinessException>(() => carteira.Debitar(0));
        }

        [Fact]
        public void CarteiraShouldThrowBusinessException_DebitarNegativo()
        {
            Assert.Throws<BusinessException>(() => carteira.Debitar(-10));
        }

        [Fact]
        public void CarteiraShouldThrowBusinessException_Creditar0()
        {
            Assert.Throws<BusinessException>(() => carteira.Creditar(0));
        }

        [Fact]
        public void CarteiraShouldThrowBusinessException_CreditarNegativo()
        {
            Assert.Throws<BusinessException>(() => carteira.Creditar(-10));
        }
    }
}
