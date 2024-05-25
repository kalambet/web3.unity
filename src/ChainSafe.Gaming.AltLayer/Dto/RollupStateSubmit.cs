using System.Collections.Generic;

namespace ChainSafe.Gaming.AltLayer.Contracts
{
    using System.Numerics;
    using Nethereum.ABI.FunctionEncoding.Attributes;

    [Event("RollupStateSubmit")]
    public class RollupStateSubmitEventDTO : IEventDTO
    {
        [Parameter("uint256", "sessionId", 1, true)]
        public BigInteger SessionId { get; set; }

        [Parameter("address", "player", 2, true)]
        public string Player { get; set; }

        [Parameter("address", "from", 3, true)]
        public string From { get; set; }

        [Parameter("uint256[]", "ids", 4, false)]
        public List<BigInteger> Ids { get; set; }

        [Parameter("uint256[]", "values", 5, false)]
        public List<BigInteger> Values { get; set; }
    }
}