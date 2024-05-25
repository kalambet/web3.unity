using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer.Types
{
    /// <summary>
    /// Configuration settings for AltLayer rollup.
    /// </summary>
    public class AltLayerConfig
    {
        /// <summary>
        /// Flashlayer settings.
        /// </summary>
        [JsonProperty(PropertyName = "flashlayer")]
        public Flashlayer Flashlayer { get; set; } = new Flashlayer();

        /// <summary>
        /// Indicates if the request is for a free trial.
        /// </summary>
        [JsonProperty(PropertyName = "freeTrial")]
        public bool FreeTrial { get; set; } = true;
    }

    /// <summary>
    /// Flashlayer configuration.
    /// </summary>
    public class Flashlayer
    {
        /// <summary>
        /// Flashlayer settings.
        /// </summary>
        [JsonProperty(PropertyName = "settings")]
        public FlashlayerSettings Settings { get; set; } = new FlashlayerSettings();

        /// <summary>
        /// Name of the flashlayer.
        /// </summary>
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; } = "freetrial";
    }

    /// <summary>
    /// Flashlayer settings.
    /// </summary>
    public class FlashlayerSettings
    {
        /// <summary>
        /// Indicates if the rollup uses FCFS mode.
        /// </summary>
        [JsonProperty(PropertyName = "fcfs")]
        public bool Fcfs { get; set; } = true;

        /// <summary>
        /// Indicates if the rollup is gasless.
        /// </summary>
        [JsonProperty(PropertyName = "gasless")]
        public bool Gasless { get; set; } = true;

        /// <summary>
        /// Block time in seconds.
        /// </summary>
        [JsonProperty(PropertyName = "blockTime")]
        public string BlockTime { get; set; } = "0.5";

        /// <summary>
        /// Symbol of the token.
        /// </summary>
        [JsonProperty(PropertyName = "tokenSymbol")]
        public string TokenSymbol { get; set; } = "ETH";

        /// <summary>
        /// Block gas limit in wei.
        /// </summary>
        [JsonProperty(PropertyName = "blockGasLimit")]
        public string BlockGasLimit { get; set; } = "70000000";

        /// <summary>
        /// Token decimals.
        /// </summary>
        [JsonProperty(PropertyName = "tokenDecimals")]
        public string TokenDecimals { get; set; } = "18";

        /// <summary>
        /// List of genesis accounts.
        /// </summary>
        [JsonProperty(PropertyName = "genesisAccounts")]
        public GenesisAccount[] GenesisAccounts { get; set; } = new GenesisAccount[]
        {
            new GenesisAccount { Account = "0x55085B2Fd83323d98d30d6B3342cc39de6D527f8", Balance = "21000000000000000000000000" },
            new GenesisAccount { Account = "0x9434e7d062bF1257BF726a96A83fAE177703ccFD", Balance = "21000000000000000000000000" }
        };
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
