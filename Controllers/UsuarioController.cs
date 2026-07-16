using Microsoft.AspNetCore.Mvc;
using PicPay.Domains.Usuarios;
using PicPay.Filters;
using PicPay.Services.UsuarioServices;

namespace PicPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Produces("application/json")]
    public class UsuarioController : ControllerBase
    {

        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<Usuario>> PostAsync([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await _usuarioService.SaveUsuarioAsync(usuarioDTO);

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        [HttpPost("{id}/imagem")]
        [ValidateImageFile]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status413PayloadTooLarge, Type = typeof(ProblemDetails))]
        [ProducesResponseType(StatusCodes.Status415UnsupportedMediaType, Type = typeof(ProblemDetails))]
        public async Task<ActionResult> PostUsuarioImagem([FromForm] IFormFile file, Guid id)
        {
            var usuario = await _usuarioService.SaveUsuarioImagemAsync(file, id);
            return CreatedAtAction("GetUsuario", new {id = id});
        }

        [HttpGet("{id}", Name = "GetUsuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _usuarioService.FindUsuarioAsync(id);

            if(usuario == null)
            {
                return NotFound($"Usuario não encontrado {id}");
            }

            return Ok(usuario);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteUsuario(Guid id)
        {
            await _usuarioService.DeleteUsuarioAsync(id);
            return Ok($"Usuário com ID {id} excluído com sucesso.");
        }
    }
}
