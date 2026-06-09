using PicPay.Domains;

namespace PicPay.Services
{
    public interface ITransacaoService
    {
        public Task<TransacaoDTO?> FindById(Guid id);
        public Task<ICollection<TransacaoDTO>> FindByPayerId(Guid id);
        public Task<ICollection<TransacaoDTO>> FindByPayeeId(Guid id);
        public Task<ICollection<TransacaoDTO>> FindByAnyId(Guid id);
        public Task<TransacaoDTO> Processar(TransacaoDTO transacaoDTO);
    }
}
