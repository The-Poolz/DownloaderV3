namespace DownloaderV3.Source.CovalentDocument.Helpers;

/// <summary>
/// Provides a collection of exception message templates.
/// </summary>
public static class ExceptionMessages
{
    /// <summary>
    /// Message indicating an index out of range error during chunk extraction.
    /// </summary>
    public const string IndexOutOfRangeChunkExtraction = "Unable to extract chunk, index is out of line.";

    /// <summary>
    /// Message indicating an invalid 'path' format in 'RawDataDecoder'.
    /// </summary>
    public const string InvalidPathInRawDataDecoder =
        "Invalid 'path' in 'RawDataDecoder' decoder. Expected format: '$data.raw_log_data#3'.";

    /// <summary>
    /// Message indicating that a class specification is missing or incorrect.
    /// </summary>
    public const string ClassSpecificationError = "The class is missing or incorrectly specified: {0}";
}