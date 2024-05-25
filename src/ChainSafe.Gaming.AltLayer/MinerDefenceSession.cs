using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

namespace ChainSafe.Gaming.AltLayer.Contracts
{
    public class MinerDefenceSession
    {
        private const string MethodMoveResources = "moveResources";
        private const string MethodMine = "mine";
        private const string MethodResolve = "resolve";
        private const string MinerDefenceSessionAbi = @"[
            {
                ""inputs"": [
                    {
                        ""internalType"": ""address"",
                        ""name"": ""new_miner"",
                        ""type"": ""address""
                    },
                    {
                        ""internalType"": ""address"",
                        ""name"": ""new_defender"",
                        ""type"": ""address""
                    }
                ],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""constructor""
            },
            {
                ""inputs"": [],
                ""name"": ""miner"",
                ""outputs"": [
                    {
                        ""internalType"": ""address"",
                        ""name"": """",
                        ""type"": ""address""
                    }
                ],
                ""stateMutability"": ""view"",
                ""type"": ""function""
            },
            {
                ""inputs"": [],
                ""name"": ""defender"",
                ""outputs"": [
                    {
                        ""internalType"": ""address"",
                        ""name"": """",
                        ""type"": ""address""
                    }
                ],
                ""stateMutability"": ""view"",
                ""type"": ""function""
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""address"",
                        ""name"": ""from_miner"",
                        ""type"": ""address""
                    },
                    {
                        ""internalType"": ""address"",
                        ""name"": ""to_defender"",
                        ""type"": ""address""
                    },
                    {
                        ""internalType"": ""uint256[]"",
                        ""name"": ""ids"",
                        ""type"": ""uint256[]""
                    },
                    {
                        ""internalType"": ""uint256[]"",
                        ""name"": ""values"",
                        ""type"": ""uint256[]""
                    },
                    {
                        ""internalType"": ""bytes"",
                        ""name"": ""data"",
                        ""type"": ""bytes""
                    }
                ],
                ""name"": ""moveResources"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""address"",
                        ""name"": ""to_miner"",
                        ""type"": ""address""
                    },
                    {
                        ""internalType"": ""uint256"",
                        ""name"": ""id"",
                        ""type"": ""uint256""
                    },
                    {
                        ""internalType"": ""uint256"",
                        ""name"": ""value"",
                        ""type"": ""uint256""
                    },
                    {
                        ""internalType"": ""bytes"",
                        ""name"": ""data"",
                        ""type"": ""bytes""
                    }
                ],
                ""name"": ""mine"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            },
            {
                ""inputs"": [],
                ""name"": ""resolve"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            }
        ]";

        private readonly Contract contract;

        public MinerDefenceSession(IRpcProvider rpcProvider, IContractBuilder cb, string contractAddress)
        {
            this.contract = cb.Build(MinerDefenceSessionAbi, contractAddress);
        }

        public async Task<TransactionReceipt> MoveResourcesAsync(string fromMiner, string toDefender, uint[] ids, uint[] values, byte[] data)
        {
            var parameters = new object[] { fromMiner, toDefender, ids, values, data };
            var (_, receipt) = await contract.SendWithReceipt(MethodMoveResources, parameters);
            return receipt;
        }

        public async Task<TransactionReceipt> MineAsync(string toMiner, uint id, uint value, byte[] data)
        {
            var parameters = new object[] { toMiner, id, value, data };
            var (_, receipt) = await contract.SendWithReceipt(MethodMine, parameters);
            return receipt;
        }

        public async Task<TransactionReceipt> ResolveAsync()
        {
            var (_, receipt) = await contract.SendWithReceipt(MethodResolve);
            return receipt;
        }
    }
}
