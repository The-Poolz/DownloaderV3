using DownloaderV3.Destination.Models;
using DownloaderV3.Source.CovalentDocument.Decoders.DecoderHelpers;

namespace DownloaderV3.Source.CovalentDocument.Decoders;

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