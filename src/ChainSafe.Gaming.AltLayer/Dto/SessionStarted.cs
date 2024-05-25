namespace ChainSafe.Gaming.AltLayer.Contracts
{
    using System.Numerics;
    using Nethereum.ABI.FunctionEncoding.Attributes;

    [Event("SessionStarted")]
    public class SessionStartedEventDTO : IEventDTO
    {
        [Parameter("uint256", "id", 1, false)]
        public BigInteger Id { get; set; }

        [Parameter("address", "miner", 2, true)]
        public string Miner { get; set; }

        [Parameter("address", "defender", 3, true)]
        public string Defender { get; set; }

        [Parameter("string", "rpcUrl", 4, false)]
        public string RpcUrl { get; set; }
    }
}