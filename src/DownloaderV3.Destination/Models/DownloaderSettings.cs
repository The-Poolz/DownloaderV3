namespace DownloaderV3.Destination.Models;

public class DownloaderSettings : BaseDownloaderSettings
{
    /// <summary>
    /// Indicating whether the downloader is active or not.
    /// </summary>
    public bool Active { get; set; }

    /// <summary>
    /// representing the full URL to get the data.
    /// example: https://api.covalenthq.com/v1/[ChainId]/events/address/[ContractAddress]/?starting-block=[StartingBlock]&ending-block=[EndingBlock]&page-number=[PageNumber]&page-size=[MaxPageNumber]&key=[Key]
    /// </summary>
    public string UrlSet { get; set; } = null!;

    public virtual ICollection<DownloaderMapping> DownloaderMappings { get; set; } = new List<DownloaderMapping>();
}