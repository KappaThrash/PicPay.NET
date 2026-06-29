using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PicPay.Domains;
using PicPay.Services;
using PicPay.Services.Auth;

namespace PicPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {

        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IJwtTokenService _jwtTokenService;

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService, IJwtTokenService jwtTokenService)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostAsync([FromBody] UsuarioDTO usuarioDTO)
        {
            var usuario = await _usuarioService.SaveUsuarioAsync(usuarioDTO);

            return CreatedAtAction("GetUsuario", new {id = usuario.Id }, usuario);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthTokenDTO>> LoginAsync([FromBody] LoginDTO loginDTO)
        {
            var usuario = await _usuarioService.AuthenticateAsync(loginDTO);

            if (usuario == null)
            {
                return Unauthorized();
            }

            return Ok(_jwtTokenService.GenerateToken(usuario));
        }

        [HttpGet("{id}", Name = "GetUsuario")]
        public async Task<ActionResult<Usuario>> GetUsuario(Guid id)
        {
            var usuario = await _usuarioService.FindUsuarioAsync(id);
            
            return Ok(usuario);
        }
    }
}
