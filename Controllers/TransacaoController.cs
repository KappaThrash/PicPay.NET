using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PicPay.Domains;
using PicPay.Services;

namespace PicPay.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransacaoController(ITransacaoService _transacaoService, ILogger<TransacaoController> logger) : Controller
    {
        [HttpPost]
        public async Task<ActionResult<TransacaoDTO>> PostTransacao([FromBody] TransacaoDTO transacaoDTO){
            var transacao = await _transacaoService.Processar(transacaoDTO);

            return CreatedAtAction(nameof(GetTransacao), new { id = transacao.Id }, transacao);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TransacaoDTO>> GetTransacao(Guid id)
        {
            return Ok( await _transacaoService.FindById(id));
        }

        [HttpGet("payer/{id}")]
        public async Task<ActionResult<ICollection<TransacaoDTO>>> GetTransacaoByPayer(Guid id)
        {
            return Ok(await _transacaoService.FindByPayerId(id));
        }

        [HttpGet("payee/{id}")]
        public async Task<ActionResult<ICollection<TransacaoDTO>>> GetTransacaoByPayee(Guid id)
        {
            return Ok(await _transacaoService.FindByPayeeId(id));
        }

        [HttpGet("any/{id}")]
        public async Task<ActionResult<ICollection<TransacaoDTO>>> GetTransacaoByAny(Guid id)
        {
            return Ok(await _transacaoService.FindByAnyId(id));
        }
    }
}
