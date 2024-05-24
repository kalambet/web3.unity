using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer.Types
{
    /// <summary>
    /// Configuration settings for AltLayer rollup.
    /// </summary>
    public class AltLayerConfig
    {
        /// <summary>
        /// Block time in seconds.
        /// </summary>
        [JsonProperty(PropertyName = "blockTime")]
        public string BlockTime { get; set; }

        /// <summary>
        /// Indicates if the rollup is gasless.
        /// </summary>
        [JsonProperty(PropertyName = "gasless")]
        public bool Gasless { get; set; }

        /// <summary>
        /// Indicates if the rollup uses FCFS mode.
        /// </summary>
        [JsonProperty(PropertyName = "fcfs")]
        public bool Fcfs { get; set; }

        /// <summary>
        /// Symbol of the token.
        /// </summary>
        [JsonProperty(PropertyName = "tokenSymbol")]
        public string TokenSymbol { get; set; }

        /// <summary>
        /// Block gas limit in wei.
        /// </summary>
        [JsonProperty(PropertyName = "blockGasLimit")]
        public string BlockGasLimit { get; set; }

        /// <summary>
        /// Token decimals.
        /// </summary>
        [JsonProperty(PropertyName = "tokenDecimals")]
        public string TokenDecimals { get; set; }

        /// <summary>
        /// List of genesis accounts.
        /// </summary>
        [JsonProperty(PropertyName = "genesisAccounts")]
        public GenesisAccount[] GenesisAccounts { get; set; }

        /// <summary>
        /// Name of the rollup.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Represents a genesis account.
    /// </summary>
    public class GenesisAccount
    {
        /// <summary>
        /// Account address.
        /// </summary>
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        /// <summary>
        /// Balance of the account.
        /// </summary>
        [JsonProperty(PropertyName = "balance")]
        public string Balance { get; set; }
    }
}
