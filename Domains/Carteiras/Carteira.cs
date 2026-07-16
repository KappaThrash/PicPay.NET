using PicPay.Domains.Usuarios;
using PicPay.Exceptions;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicPay.Domains.Carteiras
{
    /// <summary>
    /// Represents the user's digital wallet, storing the balance and managing debit and credit transactions.
    /// </summary>
    public class Carteira
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public decimal Balance { get; set; }
        public Guid UsuarioID { get; set; }
        public Usuario? Usuario { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Carteira"/> class.
        /// </summary>
        /// <param name="balance">The initial balance of the wallet.</param>
        /// <param name="usuarioID">The ID of the owner user.</param>
        public Carteira(decimal balance, Guid usuarioID)
        {
            Balance = balance;
            UsuarioID = usuarioID;
        }

        /// <summary>
        /// Debits a specific amount from the wallet.
        /// </summary>
        /// <param name="valor">The positive decimal amount to debit.</param>
        /// <returns>The updated wallet balance.</returns>
        /// <exception cref="BusinessException">Thrown when the amount is greater than the current balance or is less than or equal to zero.</exception>
        public Decimal Debitar(Decimal valor)
        {

            if(valor > this.Balance)
            {
                throw new BusinessException("Valor a debitar maior que o saldo na conta");
            }

            if (valor <= 0)
            {
                throw new BusinessException("Valor a debitar negativo ou zero");
            }

            this.Balance -= valor;
            return this.Balance;
        }

        /// <summary>
        /// Credits a specific amount into the wallet.
        /// </summary>
        /// <param name="valor">The positive decimal amount to credit.</param>
        /// <returns>The updated wallet balance.</returns>
        /// <exception cref="BusinessException">Thrown when the amount is less than or equal to zero.</exception>
        public Decimal Creditar(Decimal valor)
        {
            if (valor <= 0)
            {
                throw new BusinessException("Valor a creditar !");
            }

            this.Balance += valor;
            return this.Balance;
        }
    }
}
