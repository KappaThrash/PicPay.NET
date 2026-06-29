using Microsoft.AspNetCore.Mvc;
using PicPay.Domains;
using PicPay.Services;


namespace PicPay.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CarteiraController(ICarteiraService _service) : ControllerBase
    {

        // GET <CarteiraController>/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Carteira>> GetCarteira(Guid id)
        {
            Carteira? carteira = await _service.FindByIdAsync(id);

            if (carteira == null)
            {
                return NotFound();
            }

            return Ok(carteira);
        }

        // GET <CarteiraController>/user/{id}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<Carteira>> GetCarteiraByUser(Guid userId)
        {
            Carteira? carteira = await _service.FindByUserIdAsync(userId);

            if (carteira == null)
            {
                return NotFound();
            }

            return Ok(carteira);
        }

        // POST <CarteiraController>
        [HttpPost("{UserId}")]
        public async Task<ActionResult<Carteira>> Post(Guid UserId)
        {
            Carteira carteira = await _service.CreateCarteira(UserId);

            CarteiraDTO carteiraDTO = new CarteiraDTO(carteira.Id, carteira.Balance, carteira.UsuarioID);

            return CreatedAtAction(nameof(GetCarteira), new { id = carteira.Id }, carteiraDTO);
        }

        // DELETE <CarteiraController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            await _service.DeleteCarteira(id);
            return Ok();
        }
    }
}
