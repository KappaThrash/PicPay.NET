using PicPay.Repository;
using PicPay.Services;

namespace PicPay.Extesions
{
    public static class ServiceDependenciesConfig
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<ICarteiraService, CarteiraService>();
            services.AddScoped<ITransacaoService, TransacaoService>();
            return services;
        }
        
    }
}
