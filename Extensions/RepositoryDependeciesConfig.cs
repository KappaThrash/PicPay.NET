namespace PicPay.Extesions
{
    using PicPay.Repository;
    using PicPay.Services;
    using System.Runtime.CompilerServices;

    public static class RepositoryDependeciesConfig
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<ICarteiraRepository, CarteiraRepository>();
            services.AddScoped<ITransacaoRepository, TransacaoRepository>();
            return services;
        }
    }
}
