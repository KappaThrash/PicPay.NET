using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PicPay.Domains;
using PicPay.Services;

namespace PicPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public async Task<ActionResult<Usuario>> PostAsync([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await _usuarioService.SaveUsuarioAsync(usuarioDTO);

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        [HttpPost("{id}/imagem")]
        public async Task<ActionResult> PostUsuarioImagem([FromForm] IFormFile file, Guid id)
        {
            if (file.Length <= 0 || file == null)
            {
                return BadRequest("Arquivo tem tamanho 0 ou não enviado");
            }

            if (!file.ContentType.StartsWith("image/"))
            {
                return BadRequest("Arquivo não é uma imagem");
            }

            var usuario = await _usuarioService.SaveUsuarioImagemAsync(file, id);
            return CreatedAtAction("GetUsuario", new {id = id});
        }

        [HttpGet("{id}", Name = "GetUsuario")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _usuarioService.FindUsuarioAsync(id);
            
            return Ok(usuario);
        }
    }
}
