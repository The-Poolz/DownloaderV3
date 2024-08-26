namespace DownloaderContext.Models;

public class Chain
{
    /// <summary>
    /// Representing the blockchain network Id.
    /// </summary>
    public Int64 ChainId { get; set; }

    /// <summary>
    /// Representing the name of the blockchain network.
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Representing the RPC connection for the blockchain network.
    /// </summary>
    public string RpcConnection { get; set; } = null!;
}