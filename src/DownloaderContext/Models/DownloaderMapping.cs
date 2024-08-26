namespace DownloaderContext.Models
{
    /// <summary>
    /// Represents the mapping configuration for the downloader.
    /// </summary>
    public class DownloaderMapping
    {
        /// <summary>
        /// Unique identifier for the mapping configuration.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Path to the resource or file that needs to be downloaded.
        /// </summary>
        public string Path { get; set; } = null!;

        /// <summary>
        /// Converter to be applied to the downloaded data.
        /// </summary>
        public string Converter { get; set; } = null!;

        /// <summary>
        /// Name associated with this mapping configuration.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Settings associated with this mapping, linking to <see cref="DownloaderSettings"/>.
        /// </summary>
        public virtual DownloaderSettings DownloaderSettings { get; set; } = new();
    }
}