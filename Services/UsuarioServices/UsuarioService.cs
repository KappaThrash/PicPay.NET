using PicPay.Domains.Usuarios;
using PicPay.Exceptions;
using PicPay.Repository.UsuarioRepositories;

namespace PicPay.Services.UsuarioServices
{
    public class UsuarioService(IUsuarioRepository repository) : IUsuarioService
    {

        public async Task<Usuario?> FindUsuarioAsync(Guid id)
        {
            return await repository.GetByIdAsync(id);
        }

        public async Task DeleteUsuarioAsync(Guid id)
        {
            await repository.DeleteByIdAsync(id);
            await repository.SaveChangesAsync();
        }

        public async Task<Usuario> SaveUsuarioAsync(UsuarioDTO usuarioDTO)
        {
            ArgumentNullException.ThrowIfNull(usuarioDTO);
            Usuario usuario = new(usuarioDTO.Nome, usuarioDTO.Email, usuarioDTO.Tipo, 
                usuarioDTO.Cpf, usuarioDTO.DataNascimento, usuarioDTO.Senha);

            return await repository.SaveAsync(usuario);
        }

        /// <inheritdoc />
        public async Task<Usuario?> SaveUsuarioImagemAsync(IFormFile file, Guid id)
        {
            byte[] dadosImagem;
            using var memoryStream = new MemoryStream();

            await file.CopyToAsync(memoryStream);

            dadosImagem = memoryStream.ToArray();

            var usuario = await repository.GetByIdAsync(id) ?? 
                throw new UserNotFoundException("Usuario não encontrado " + nameof(SaveUsuarioImagemAsync));

            usuario.Imagem!.Bytes = dadosImagem;
            usuario.Imagem.ContentType = file.ContentType;
            usuario.Imagem.NomeImagem = $"{usuario.Id.ToString()}UsuarioImagem";

            return await repository.SaveAsync(usuario);
        }
    }
}
