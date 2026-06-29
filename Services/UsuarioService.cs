using PicPay.Domains;
using PicPay.Repository;

namespace PicPay.Services
{
    public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
    {

        public async Task<Usuario?> FindUsuarioAsync(Guid id)
        {
            return await repository.FindByIdAsync(id);
        }

        public async Task<Usuario> SaveUsuarioAsync(UsuarioDTO usuarioDTO)
        {
            ArgumentNullException.ThrowIfNull(usuarioDTO);
            Usuario usuario = new(usuarioDTO.Nome, usuarioDTO.Email, usuarioDTO.Tipo, usuarioDTO.Cpf, usuarioDTO.DataNascimento, usuarioDTO.Senha);
            return await repository.SaveAsync(usuario);
        }

        public async Task<Usuario?> AuthenticateAsync(LoginDTO loginDTO)
        {
            ArgumentNullException.ThrowIfNull(loginDTO);

            var usuario = await repository.FindByEmailAsync(loginDTO.Email);

            if (usuario == null || usuario.Senha != loginDTO.Senha)
            {
                return null;
            }

            return usuario;
        }
    }
}
