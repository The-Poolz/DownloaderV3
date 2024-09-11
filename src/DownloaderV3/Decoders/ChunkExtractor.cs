using DownloaderV3.Helpers;
using Newtonsoft.Json.Linq;
using Nethereum.Hex.HexTypes;
using System.Text.RegularExpressions;

namespace DownloaderV3.Decoders;

public class ChunkExtractor
{
    private const int ChunkSize = 64;
    private readonly int _chunkIndex;
    private readonly string _rawData;
    public string TopicData => new HexUTF8String(ExtractChunk).Value;
    public ChunkExtractor(string path, JToken source)
    {
        (var dataPath, _chunkIndex) = ExtractDataPathAndChunkIndex(path);
        _rawData = source.SelectToken(dataPath)!.ToString();
        if (_rawData.Length < ChunkSize * _chunkIndex)
        {
            ApplicationLogger.LogAndThrow(new IndexOutOfRangeException(ExceptionMessages.IndexOutOfRangeChunkExtraction));
        }
        if (_rawData.StartsWith("0x"))
        {
            _rawData = _rawData[2..];
        }
    }
    private int ChunkStartIndex => _chunkIndex * ChunkSize;
    private string ExtractChunk => _rawData[(ChunkStartIndex - ChunkSize)..ChunkStartIndex];
    public static (string DataPath, int ChunkIndex) ExtractDataPathAndChunkIndex(string fullPath)
    {
        var match = Regex.Match(fullPath, @"^(.*)#(\d+)$");
        if (!match.Success)
        {
            ApplicationLogger.LogAndThrow(new InvalidOperationException(ExceptionMessages.InvalidPathInRawDataDecoder));
        }

        return (match.Groups[1].Value, int.Parse(match.Groups[2].Value));
    }
}