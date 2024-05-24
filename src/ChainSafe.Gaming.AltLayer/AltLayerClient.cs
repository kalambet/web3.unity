using System;
using System.Threading.Tasks;
using ChainSafe.Gaming.AltLayer.Types;
using ChainSafe.Gaming.Web3.Environment;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer
{
    public interface IAltLayerClient
    {
        Task<string> CreateRollupAsync();
    }

    public class AltLayerClient : IAltLayerClient
    {
        private readonly IHttpClient _httpClient;
        private readonly AltLayerConfig _config;
        private const string AltLayerUrl = "https://api.altlayer.io/flashlayer";

        public AltLayerClient(IHttpClient httpClient, AltLayerConfig config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> CreateRollupAsync()
        {
            var request = new RollupRequest
            {
                Flashlayer = new Flashlayer
                {
                    Settings = new Settings
                    {
                        Fcfs = _config.Fcfs,
                        Gasless = _config.Gasless,
                        BlockTime = _config.BlockTime,
                        TokenSymbol = _config.TokenSymbol,
                        BlockGasLimit = _config.BlockGasLimit,
                        TokenDecimals = _config.TokenDecimals,
                        GenesisAccounts = _config.GenesisAccounts,
                    },
                    Name = _config.Name,
                },
                FreeTrial = true,
            };

            var requestBody = JsonConvert.SerializeObject(request);

            var response = await _httpClient.PostRaw(AltLayerUrl, requestBody, "application/json");
            response.AssertSuccess();

            var rollupResponse = JsonConvert.DeserializeObject<RollupResponse>(response.Response);

            return rollupResponse.Flashlayer.Resources.Rpc;
        }
    }

    public class RollupRequest
    {
        [JsonProperty(PropertyName = "flashlayer")]
        public Flashlayer Flashlayer { get; set; }

        [JsonProperty(PropertyName = "freeTrial")]
        public bool FreeTrial { get; set; }
    }
}
