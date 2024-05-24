using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer.Types
{
    /// <summary>
    /// Represents the response from the AltLayer rollup creation.
    /// </summary>
    public class RollupResponse
    {
        [JsonProperty(PropertyName = "flashlayer")]
        public Flashlayer Flashlayer { get; set; }
    }

    public class Flashlayer
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        [JsonProperty(PropertyName = "settings")]
        public Settings Settings { get; set; }

        [JsonProperty(PropertyName = "resources")]
        public Resources Resources { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tier")]
        public string Tier { get; set; }
    }

    public class Settings
    {
        [JsonProperty(PropertyName = "fcfs")]
        public bool Fcfs { get; set; }

        [JsonProperty(PropertyName = "gasless")]
        public bool Gasless { get; set; }

        [JsonProperty(PropertyName = "blockTime")]
        public string BlockTime { get; set; }

        [JsonProperty(PropertyName = "tokenSymbol")]
        public string TokenSymbol { get; set; }

        [JsonProperty(PropertyName = "blockGasLimit")]
        public string BlockGasLimit { get; set; }

        [JsonProperty(PropertyName = "tokenDecimals")]
        public string TokenDecimals { get; set; }

        [JsonProperty(PropertyName = "genesisAccounts")]
        public GenesisAccount[] GenesisAccounts { get; set; }
    }

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
