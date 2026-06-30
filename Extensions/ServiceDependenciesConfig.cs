using PicPay.Repository;
using PicPay.Services.CarteiraServices;
using PicPay.Services.TransacaoServices;
using PicPay.Services.UsuarioServices;

namespace PicPay.Extensions
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
