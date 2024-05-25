using ChainSafe.Gaming.AltLayer.Types;
using ChainSafe.Gaming.Web3.Build;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ChainSafe.Gaming.AltLayer
{
    public static class AltLayerExtensions
    {
        public static IWeb3ServiceCollection UseAltLayer(this IWeb3ServiceCollection services, AltLayerConfig configuration)
        {
            // todo extract interfaces of AltLayerConfig & AltLayerClient

            services.Replace(ServiceDescriptor.Singleton(typeof(AltLayerConfig), configuration));
            services.AddSingleton<AltLayerClient>();

            return services;
        }

        public static AltLayerClient AltLayer(this Web3.Web3 web3)
        {
            return web3.ServiceProvider.GetRequiredService<AltLayerClient>();
        }
    }
}