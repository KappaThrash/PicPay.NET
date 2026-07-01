using Microsoft.AspNetCore.Mvc;
using PicPay.Domains;
using PicPay.Services.UsuarioServices;

namespace PicPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IConfiguration _configuration;
        private readonly long _maxFileSize;
        private readonly IReadOnlyCollection<string> _allowedImageTypes;
        private readonly IReadOnlyCollection<string> _allowedImageExtensions;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService, IConfiguration configuration)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _configuration = configuration;
            _maxFileSize = _configuration.GetValue<long>("ImageSettings:MaxFileSize");
            _allowedImageTypes = _configuration.GetSection("ImageSettings:AllowedImageTypes").Get<IReadOnlyCollection<string>>()!;
            _allowedImageExtensions = _configuration.GetSection("ImageSettings:AllowedImageExtensions").Get<IReadOnlyCollection<string>>()!;
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
            string extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            if (file == null || file.Length <= 0)
            {
                return BadRequest("Arquivo tem tamanho 0 ou não enviado");
            }

            if (file.Length > _maxFileSize)
            {
                return StatusCode(StatusCodes.Status413PayloadTooLarge, "Arquivo passou do limite (10Mb)");
            }

            if (!file.ContentType.StartsWith("image/") || !_allowedImageTypes.Contains(file.ContentType))
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, "Tipo de arquivo não suportado para esse endpoint");
            }
            
            if (!_allowedImageExtensions.Contains(extension))
            {
                return StatusCode(StatusCodes.Status415UnsupportedMediaType, "Extensão de arquivo não suportada para esse endpoint");
            }

            var usuario = await _usuarioService.SaveUsuarioImagemAsync(file, id);
            return CreatedAtAction("GetUsuario", new {id = id});
        }

        [HttpGet("{id}", Name = "GetUsuario")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _usuarioService.FindUsuarioAsync(id);

            if(usuario == null)
            {
                return NotFound($"Usuario não encontrado {id}");
            }

            return Ok(usuario);
        }
    }
}
