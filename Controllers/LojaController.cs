using Microsoft.AspNetCore.Mvc;
using PicPay.Domains.Lojas;
using PicPay.Repository.LojaRepositories;
using PicPay.Services.LojaServices;

namespace PicPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LojaController(ILojaService lojaService, ILojaRepository lojaRepository) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateLoja([FromBody] LojaDTO lojaDTO)
        {
            var loja = await lojaService.CreateLojaAsync(lojaDTO);

            return CreatedAtAction(nameof(GetLojaById), new { id = loja.Id }, loja);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Loja>> GetLojaById(Guid id)
        {
            var loja = await lojaService.GetLojaByIdAsync(id);
            return Ok(loja);
        }
        [HttpGet("/usuario/{id}")]
        public async Task<ActionResult<Loja>> GetLojaByUserId(Guid id)
        {
            var loja = await lojaRepository.GetByUserId(id);
            return Ok(loja);
        }
    }
}
