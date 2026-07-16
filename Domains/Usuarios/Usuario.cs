using PicPay.Domains.Carteiras;
using PicPay.Domains.Utils;
using System.ComponentModel.DataAnnotations.Schema;

namespace PicPay.Domains.Usuarios
{
    /// <summary>
    /// Represents a system user account, which can be either a standard user or a merchant (lojista).
    /// </summary>
    public class Usuario
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public TipoEnum Tipo { get; set; }
        public string? Cpf { get; set; }
        public DateOnly DataNascimento { get; set; }
        public string? Senha { get; set; }
        public Imagem? Imagem { get; set; } = null;
        public Carteira? carteira { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Usuario"/> class.
        /// </summary>
        /// <param name="nome">The full name of the user.</param>
        /// <param name="email">The unique email address of the user.</param>
        /// <param name="tipo">The type of the user account (USUARIO or LOJISTA).</param>
        /// <param name="cpf">The unique CPF or CNPJ identification string.</param>
        /// <param name="dataNascimento">The user's date of birth.</param>
        /// <param name="senha">The account password.</param>
        public Usuario(string nome, string email, TipoEnum tipo, string cpf, DateOnly dataNascimento, string senha)
        {
            this.Nome = nome;
            this.Email = email;
            this.Tipo = tipo;
            this.Cpf = cpf;
            this.DataNascimento = dataNascimento;
            this.Senha = senha;
        }

        /// <summary>
        /// Checks whether the user is registered as a merchant (lojista).
        /// </summary>
        /// <returns>True if the user is a merchant; otherwise, false.</returns>
        public bool IsLojista()
        {
            return this.Tipo == TipoEnum.LOJISTA;
        }
    }
}
