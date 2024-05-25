using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChainSafe.Gaming.Evm.Contracts;
using ChainSafe.Gaming.Evm.Contracts.Extensions;
using ChainSafe.Gaming.Evm.Providers;
using ChainSafe.Gaming.Web3;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using TransactionReceipt = ChainSafe.Gaming.Evm.Transactions.TransactionReceipt;

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
        private readonly IRpcProvider rpcProvider;

        public SessionManager(IRpcProvider rpcProvider, IContractBuilder cb, string contractAddress)
        {
            this.contract = cb.Build(SessionManagerAbi, address);
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
                throw new Web3Exception("No \"RewardsClaimed\" events were found in log's receipt.");
            }
            return eventLogs.First().Event;
        }

        public async Task<string> ListenForSessionAsync()
        {
            var latestBlock = await rpcProvider.GetBlock();
            latestBlock.Number
            var logs = contract.GetEventLogs("SessionStarted");
            return "";
        }
    }
}
