namespace PicPay.Extensions
{
    using PicPay.Repository.CarteiraRepositories;
    using PicPay.Repository.Template;
    using PicPay.Repository.TransacaoRepositories;
    using PicPay.Repository.UsuarioRepositories;
    using PicPay.Repository.LojaRepositories;
    using PicPay.Repository.ProdutoRepositories;

    public static class RepositoryDependeciesConfig
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBasicRepository<>), typeof(BasicRepository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICarteiraRepository, CarteiraRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            services.AddScoped<ILojaRepository, LojaRepository>();
            services.AddScoped<IProdutoRepository, ProdutoRepository>();
            return services;
        }
    }
}
