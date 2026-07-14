namespace PicPay.Extensions
{
    using PicPay.Repository.CarteiraRepositories;
    using PicPay.Repository.Template;
    using PicPay.Repository.TransacaoRepositories;
    using PicPay.Repository.UsuarioRepositories;

    public static class RepositoryDependeciesConfig
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IBasicRepository<>), typeof(BasicRepository<>));
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICarteiraRepository, CarteiraRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            return services;
        }
    }
}
