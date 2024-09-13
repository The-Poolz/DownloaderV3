using System.Text.RegularExpressions;

namespace DownloaderV3.Source.CovalentDocument.Decoders.DecoderHelpers;

public class DecoderParameters
{
    public string? DecoderInDecoder { get; }
    public string Decoder { get; }

    public DecoderParameters(string converter)
    {
        try
        {
            Decoder = converter;
            DecoderInDecoder = null;
            var match = Regex.Match(converter, @"^([^()]+)\(([^)]+)\)$", RegexOptions.None, TimeSpan.FromMilliseconds(1000));

            if (match.Success)
            {
                Decoder = match.Groups[1].Value;
                DecoderInDecoder = match.Groups[2].Value;
            }
        }
        catch (RegexMatchTimeoutException)
        {
            Decoder = converter;
            DecoderInDecoder = null;
        }
    }
}