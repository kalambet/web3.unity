using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer.Types
{
    /// <summary>
    /// Represents the response from the AltLayer rollup creation.
    /// </summary>
    public class RollupResponse
    {
        [JsonProperty(PropertyName = "flashlayer")]
        public FlashlayerConfiguration Flashlayer { get; set; }
    }

    /// <summary>
    /// Flashlayer details in the response.
    /// </summary>
    public class FlashlayerConfiguration
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "settings")]
        public FlashlayerSettings Settings { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public Resources Resources { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tier")]
        public string Tier { get; set; }
    }

    /// <summary>
    /// Resources provided in the response.
    /// </summary>
    public class Resources
    {
        [JsonProperty(PropertyName = "rpc")]
        public string Rpc { get; set; }

        [JsonProperty(PropertyName = "explorer")]
        public string Explorer { get; set; }

        [JsonProperty(PropertyName = "faucet")]
        public string Faucet { get; set; }

        [JsonProperty(PropertyName = "chainId")]
        public string ChainId { get; set; }

        [JsonProperty(PropertyName = "rpcWs")]
        public string RpcWs { get; set; }
    }
}
