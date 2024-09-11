using DownloaderV3.Destination.Attributes;

namespace DownloaderV3.Destination.Models;

public class BaseDownloaderSettings
{
    /// <summary>
    /// Representing the blockchain network Id.
    /// </summary>
    public Int64 ChainId { get; set; }

    /// <summary>
    /// Representing the contract address.
    /// </summary>
    public string ContractAddress { get; set; } = null!;

    /// <summary>
    /// Representing the event hash.
    /// </summary>
    public string EventHash { get; set; } = null!;

    /// <summary>
    /// Representing the starting block for the downloading data range.
    /// </summary>
    public Int64 StartingBlock { get; set; }

    /// <summary>
    /// representing the ending block for the downloading data range.
    /// </summary>
    public Int64 EndingBlock { get; set; }

    /// <summary>
    /// representing the maximum batch size for processing data.
    /// </summary>
    public Int64 MaxBatchSize { get; set; }

    /// <summary>
    /// representing the Covalent key.
    /// </summary>
    public string Key { get; set; } = null!;

    /// <summary>
    /// Representing the name of the class that use <see cref="ResponseModelAttribute"/> attribute for handling the response.
    /// </summary>
    public string ResponseType { get; set; } = null!;

    /// <summary>
    /// representing the maximum pagination pages number.
    /// </summary>
    public int MaxPageNumber { get; set; }
}