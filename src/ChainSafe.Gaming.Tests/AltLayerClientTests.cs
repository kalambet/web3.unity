using System.Threading.Tasks;
using ChainSafe.Gaming.AltLayer;
using ChainSafe.Gaming.AltLayer.Types;
using ChainSafe.Gaming.Web3;
using ChainSafe.Gaming.Web3.Environment;
using Moq;
using Newtonsoft.Json;
using Xunit;

public class AltLayerClientTests
{
    private readonly Mock<IHttpClient> mockHttpClient;
    private readonly AltLayerConfig config;
    private readonly AltLayerClient altLayerClient;

    public AltLayerClientTests()
    {
        mockHttpClient = new Mock<IHttpClient>();
        config = new AltLayerConfig
        {
            Flashlayer = new Flashlayer
            {
                Name = "freetrial",
                Settings = new FlashlayerSettings
                {
                    BlockTime = "0.5",
                    Gasless = true,
                    Fcfs = true,
                    TokenSymbol = "ETH",
                    BlockGasLimit = "70000000",
                    TokenDecimals = "18",
                    GenesisAccounts = new[]
                    {
                        new GenesisAccount { Account = "0x55085B2Fd83323d98d30d6B3342cc39de6D527f8", Balance = "21000000000000000000000000" },
                        new GenesisAccount { Account = "0x9434e7d062bF1257BF726a96A83fAE177703ccFD", Balance = "21000000000000000000000000" },
                    },
                },
            },
            FreeTrial = true,
        };
        altLayerClient = new AltLayerClient(mockHttpClient.Object, config);
    }

    [Fact]
    public async Task CreateRollupAsync_ShouldReturnRpcEndpoint_WhenRequestIsSuccessful()
    {
        // Arrange
        var expectedRpcEndpoint = "https://flashlayer.alt.technology/freetrial";
        var rollupResponse = new RollupResponse
        {
            Flashlayer = new FlashlayerConfiguration
            {
                Resources = new Resources
                {
                    Rpc = expectedRpcEndpoint,
                },
            },
        };
        var responseContent = JsonConvert.SerializeObject(rollupResponse);
        var networkResponse = NetworkResponse<string>.Success(responseContent);
        mockHttpClient.Setup(client => client.PostRaw(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(networkResponse);

        // Act
        var rpcEndpoint = await altLayerClient.CreateRollupAsync();

        // Assert
        Assert.Equal(expectedRpcEndpoint, rpcEndpoint);
    }

    [Fact]
    public async Task CreateRollupAsync_ShouldThrowException_WhenRequestFails()
    {
        // Arrange
        var networkResponse = NetworkResponse<string>.Failure("Error occurred");
        mockHttpClient.Setup(client => client.PostRaw(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(networkResponse);

        // Act & Assert
        await Assert.ThrowsAsync<Web3Exception>(() => altLayerClient.CreateRollupAsync());
    }
}
