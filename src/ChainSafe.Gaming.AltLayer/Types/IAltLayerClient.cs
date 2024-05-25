using System.Threading.Tasks;


namespace ChainSafe.Gaming.AltLayer.Types
{
    public interface IAltLayerClient
    {
        Task<string> CreateRollupAsync();
    }
}