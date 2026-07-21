using PicPay.Domains.Lojas;
using PicPay.Repository.DataContext;
using PicPay.Repository.Template;

namespace PicPay.Repository.LojaRepositories
{
    public class LojaRepository : BasicRepository<Loja>, ILojaRepository
    {
        public LojaRepository(PicPayDbContext context) : base(context) {}
    }
}
