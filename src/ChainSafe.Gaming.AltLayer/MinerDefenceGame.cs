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
    public class MinerDefenceGame
    {
        private const string MethodSubmitRollupState = "submitRollupState";
        private const string MethodClaim = "claim";
        private const string MinerDefenceGameAbi = @"[
            {
                ""anonymous"": false,
                ""inputs"": [
                    {
                        ""indexed"": true,
                        ""internalType"": ""uint256"",
                        ""name"": ""sessionId"",
                        ""type"": ""uint256""
                    },
                    {
                        ""indexed"": true,
                        ""internalType"": ""address"",
                        ""name"": ""player"",
                        ""type"": ""address""
                    },
                    {
                        ""indexed"": true,
                        ""internalType"": ""address"",
                        ""name"": ""from"",
                        ""type"": ""address""
                    },
                    {
                        ""indexed"": false,
                        ""internalType"": ""uint256[]"",
                        ""name"": ""ids"",
                        ""type"": ""uint256[]""
                    },
                    {
                        ""indexed"": false,
                        ""internalType"": ""uint256[]"",
                        ""name"": ""values"",
                        ""type"": ""uint256[]""
                    }
                ],
                ""name"": ""RollupStateSubmit"",
                ""type"": ""event""
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""uint256"",
                        ""name"": ""sessionId"",
                        ""type"": ""uint256""
                    },
                    {
                        ""internalType"": ""address"",
                        ""name"": ""player"",
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
                    }
                ],
                ""name"": ""submitRollupState"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            },
            {
                ""inputs"": [
                    {
                        ""internalType"": ""uint256"",
                        ""name"": ""sessionId"",
                        ""type"": ""uint256""
                    },
                    {
                        ""internalType"": ""address"",
                        ""name"": ""player"",
                        ""type"": ""address""
                    },
                    {
                        ""internalType"": ""uint256[]"",
                        ""name"": ""ids"",
                        ""type"": ""uint256[]""
                    },
                    {
                        ""internalType"": ""bytes"",
                        ""name"": ""data"",
                        ""type"": ""bytes""
                    }
                ],
                ""name"": ""claim"",
                ""outputs"": [],
                ""stateMutability"": ""nonpayable"",
                ""type"": ""function""
            }
        ]";

        private readonly Contract contract;

        public MinerDefenceGame(IRpcProvider rpcProvider, IContractBuilder cb, string contractAddress)
        {
            this.contract = cb.Build(MinerDefenceGameAbi, contractAddress);
        }

        public async Task<RollupStateSubmitEventDTO> SubmitRollupStateAsync(uint sessionId, string player, uint[] ids, uint[] values)
        {
            var parameters = new object[] { sessionId, player, ids, values };
            var (_, receipt) = await contract.SendWithReceipt(MethodSubmitRollupState, parameters);
            var logs = receipt.Logs.Select(jToken => JsonConvert.DeserializeObject<FilterLog>(jToken.ToString()));
            var eventAbi = EventExtensions.GetEventABI<RollupStateSubmitEventDTO>();
            var eventLogs = logs
                .Select(log => eventAbi.DecodeEvent<RollupStateSubmitEventDTO>(log))
                .Where(l => l != null);

            if (!eventLogs.Any())
            {
                throw new Web3Exception("No \"RollupStateSubmit\" events were found in log's receipt.");
            }
            return eventLogs.First().Event;
        }

        public async Task<TransactionReceipt> ClaimAsync(uint sessionId, string player, uint[] ids, byte[] data)
        {
            var parameters = new object[] { sessionId, player, ids, data };
            var (_, receipt) = await contract.SendWithReceipt(MethodClaim, parameters);
            return receipt;
        }
    }
}
