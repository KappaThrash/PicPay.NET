namespace PicPay.Extensions
{
    using PicPay.Repository.CarteiraRepositories;
    using PicPay.Repository.TransacaoRepositories;
    using PicPay.Repository.UsuarioRepositories;
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
