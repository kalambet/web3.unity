using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.AltLayer.Types;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer
{
    public class AltLayerClient : IAltLayerClient
    {
        private const string AltLayerUrl = "https://api.altlayer.io/flashlayer";
        private readonly IHttpClient httpClient;
        private readonly AltLayerConfig config;

        public AltLayerClient(IHttpClient httpClient, AltLayerConfig config)
        {
            this.httpClient = httpClient;
            this.config = config;
        }

        public async Task<string> CreateRollupAsync()
        {
            var requestBody = JsonConvert.SerializeObject(config);

            var response = await httpClient.PostRaw(AltLayerUrl, requestBody, "application/json");
            response.AssertSuccess();

            var rollupResponse = JsonConvert.DeserializeObject<RollupResponse>(response.Response);

            return rollupResponse?.Flashlayer.Resources.Rpc!;
        }
    }
}
