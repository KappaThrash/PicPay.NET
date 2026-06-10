using PicPay.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicPay.Domains
{
    public class Carteira
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public Guid UsuarioID { get; set; }
        public Usuario? Usuario { get; set; }

        public Carteira(decimal balance, Guid usuarioID)
        {
            Balance = balance;
            UsuarioID = usuarioID;
        }

        public Decimal Debitar(Decimal valor)
        {
            if (valor == 0)
            {
                throw new BusinessException("Valor a debitar 0");
            }

            if (valor < 0)
            {
                throw new BusinessException("Valor a debitar negativo!");
            }

            this.Balance -= valor;
            return this.Balance;
        }

        public Decimal Creditar(Decimal valor)
        {
            if (valor == 0)
            {
                throw new BusinessException("Valor a creditar !");
            }

            if (valor < 0)
            {
                throw new BusinessException("Valor a creditar negativo!");
            }

            this.Balance += valor;
            return this.Balance;
        }
    }
}
