using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer.Contracts
{
    public class SessionManager
    {
        private const string MethodStartSession = "startSession";
        private const string MethodJoinSession = "joinSession";
        private const string SessionManagerAbi = @"[
            {
                ""anonymous"": false,
                ""inputs"": [
                    {
                        ""indexed"": true,
                        ""internalType"": ""address"",
                        ""name"": ""miner"",
                        ""type"": ""address""
                    },
                    {
                        ""indexed"": true,
                        ""internalType"": ""address"",
                        ""name"": ""defender"",
                        ""type"": ""address""
                    },
                    {
                        ""indexed"": false,
                        ""internalType"": ""string"",
                        ""name"": ""rpcUrl"",
                        ""type"": ""string""
                    }
                ],
                ""name"": ""SessionStarted"",
                ""type"": ""event""
            },
            {
                ""anonymous"": false,
                ""inputs"": [
                    {
                        ""indexed"": true,
                        ""internalType"": ""uint256"",
                        ""name"": ""id"",
                        ""type"": ""uint256""
                    },
                    {
                        ""indexed"": true,
                        ""internalType"": ""address"",
                        ""name"": ""miner"",
                        ""type"": ""address""
                    },
                    {
                        ""indexed"": true,
                        ""internalType"": ""address"",
                        ""name"": ""defender"",
                        ""type"": ""address""
                    },
                    {
                        ""indexed"": false,
                        ""internalType"": ""string"",
                        ""name"": ""rpcUrl"",
                        ""type"": ""string""
                    }
                ],
                ""name"": ""SessionJoined"",
                ""type"": ""event""
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""address"",
                        ""name"": ""defender"",
                        ""type"": ""address""
                    },
                    {
                        ""internalType"": ""string"",
                        ""name"": ""rpcUrl"",
                        ""type"": ""string""
                    }
                ],
                ""name"": ""startSession"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""address"",
                        ""name"": ""miner"",
                        ""type"": ""address""
                    }
                ],
                ""name"": ""joinSession"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            }
        ]";

        private readonly Contract contract;
        private readonly string address;
        private readonly IRpcProvider rpcProvider;

        public SessionManager(IRpcProvider rpcProvider, IContractBuilder cb, string contractAddress)
        {
            this.contract = cb.Build(SessionManagerAbi, contractAddress);
            this.address = contractAddress;
            this.rpcProvider = rpcProvider;
        }

        public async Task<SessionStartedEventDTO> StartSessionAsync(string defender, string rpcUrl)
        {
            var parameters = new object[] { defender, rpcUrl };
            var (_, receipt) = await contract.SendWithReceipt(MethodStartSession, parameters);
            var logs = receipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
            var eventAbi = EventExtensions.GetEventABI<SessionStartedEventDTO>();
            var eventLogs = logs
                .Select(log => eventAbi.DecodeEvent<SessionStartedEventDTO>(log))
                .Where(l => l != null);

            if (!eventLogs.Any())
            {
                throw new Web3Exception("No \"SessionStarted\" events were found in log's receipt.");
            }
            return eventLogs.First().Event;
        }

        public async Task<SessionJoinedEventDTO> JoinSessionAsync(string miner)
        {
            var parameters = new object[] { miner };
            var (_, receipt) = await contract.SendWithReceipt(MethodJoinSession, parameters);
            var logs = receipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
            var eventAbi = EventExtensions.GetEventABI<SessionJoinedEventDTO>();
            var eventLogs = logs
                .Select(log => eventAbi.DecodeEvent<SessionJoinedEventDTO>(log))
                .Where(l => l != null);

            if (!eventLogs.Any())
            {
                throw new Web3Exception("No \"SessionJoined\" events were found in log's receipt.");
            }
            return eventLogs.First().Event;
        }
    }
}
