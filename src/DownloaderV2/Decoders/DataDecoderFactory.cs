using DownloaderV2.Helpers;
using Newtonsoft.Json.Linq;
using DownloaderContext.Models;

namespace DownloaderV2.Decoders;

public class DataDecoderFactory
{
    public readonly DataDecoder Decoder;

    public DataDecoderFactory(DownloaderMapping map, JToken source)
    {
        var config = new DecoderConfiguration(map);
        Decoder = GetConverter(config.Parameters.Decoder);
        Decoder.Initialize(config, source);
    }

    public static DataDecoder GetConverter(string converterName)
    {
        var typeName = $"SaveLogsByContract.Decoders.DataDecoders.{converterName}, SaveLogsByContract";
        var type = Type.GetType(typeName) ?? ApplicationLogger.LogAndThrowDynamic(new InvalidOperationException(string.Format(ExceptionMessages.DecoderTypeNotFound, converterName)));
        return (DataDecoder)Activator.CreateInstance(type)!;
    }
}