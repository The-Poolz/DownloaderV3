namespace DownloaderContext.Models;

public class ChainInfo
{
    /// <summary>
    /// Representing the blockchain network Id.
    /// </summary>
    public Int64 ChainId { get; set; }

    /// <summary>
    /// Representing the number of blocks generated per second in the blockchain network.
    /// </summary>
    public float BlockPerSecond { get; set; }

    /// <summary>
    /// The number of seconds after which the blockchain lag warning starts.
    /// </summary>
    public Int64 SecondsToWarning { get; set; }

    /// <summary>
    /// The number of seconds after which an error occurs due to a lag behind the blockchain.
    /// </summary>
    public Int64 SecondsToError { get; set; }

    /// <summary>
    /// The `DownloadTimeDelay` column is added to regulate the delay time of blockchain data downloaded from the Covalent API.<br/>
    /// Covalent API provides real-time updates for the latest blocks in the blockchain, and these updates can happen faster than the updates for other data types, such as transactions, events, and more.<br/>
    /// Which in turn leads to errors when trying to get data.<br/>
    /// This column allows setting a delay time for downloading the data, providing more flexible control over the data loading from the Covalent API, to avoid errors and ensure more accurate blockchain data processing.
    /// </summary>
    public float DownloadTimeDelay { get; set; }
}