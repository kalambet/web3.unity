using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Transactions;
using Newtonsoft.Json;

namespace ChainSafe.Gaming.AltLayer.Contracts
{
    public class SessionManager
    {
        private const string MethodStartSession = "startSession";
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
            }
        ]";

        private readonly Contract contract;
        private readonly string address;

        public SessionManager(IContractBuilder cb, string contractAddress)
        {
            this.contract = cb.Build(SessionManagerAbi, address);
            this.address = contractAddress;
        }

        public async Task<TransactionReceipt> StartSessionAsync(string defender, string rpcUrl)
        {
            var parameters = new object[] { defender, rpcUrl };
            var receipt = await contract.SendWithReceipt(MethodStartSession, parameters);
            return receipt.receipt;
        }

        public async Task<string> ListenForSessionAsync()
        {
            contract.GetEventLogs("SessionStarted");
            return "";
        }
    }

    public class SessionStartedEventDTO
    {
        [JsonProperty("miner")]
        public string Miner { get; set; }

        [JsonProperty("defender")]
        public string Defender { get; set; }

        [JsonProperty("rpcUrl")]
        public string RpcUrl { get; set; }
    }
}
