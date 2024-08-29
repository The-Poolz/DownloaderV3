using DownloaderContext.Models;
using DownloaderV2.Decoders.DecoderHelpers;

namespace DownloaderV2.Decoders;

public class DecoderConfiguration(DownloaderMapping mapping) : ICloneable
{
    public DownloaderMapping Mapping { get; } = mapping;
    public DecoderParameters Parameters { get; } = new(mapping.Converter);

    public object Clone()
    {
        var clonedMapping = new DownloaderMapping
        {
            Id = this.Mapping.Id,
            Path = this.Mapping.Path,
            Converter = this.Mapping.Converter,
            Name = this.Mapping.Name,
            DownloaderSettings = this.Mapping.DownloaderSettings
        };

        return new DecoderConfiguration(clonedMapping);
    }
}