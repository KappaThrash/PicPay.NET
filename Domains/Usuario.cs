using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace PicPay.Domains
{
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

        public Carteira? carteira { get; set; }

        public Usuario(string nome, string email, TipoEnum tipo, string cpf, DateOnly dataNascimento, string senha)
        {
            this.Nome = nome;
            this.Email = email;
            this.Tipo = tipo;
            this.Cpf = cpf;
            this.DataNascimento = dataNascimento;
            this.Senha = senha;
        }

        public bool IsLojista()
        {
            return this.Tipo == TipoEnum.LOJISTA;
        }
    }
}
