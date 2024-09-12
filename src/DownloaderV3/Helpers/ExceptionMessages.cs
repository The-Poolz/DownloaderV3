namespace DownloaderV3.Helpers;

/// <summary>
/// Provides a collection of exception message templates.
/// </summary>
public static class ExceptionMessages
{
    /// <summary>
    /// Message indicating a failure to retrieve the last block data from the Covalent API.
    /// </summary>
    public const string FailedToRetrieveLastBlockData = "Failed to retrieve last block data from the Covalent API.";

    /// <summary>
    /// Message indicating that the Failed to retrieve data from Covalent API.
    /// </summary>
    public const string FailedToRetrieveDataFromCovalentApi = "Failed to retrieve data from the Covalent API";

    /// <summary>
    /// Message indicating an index out of range error during chunk extraction.
    /// </summary>
    public const string IndexOutOfRangeChunkExtraction = "Unable to extract chunk, index is out of line.";

    /// <summary>
    /// Message indicating an invalid 'path' format in 'RawDataDecoder'.
    /// </summary>
    public const string InvalidPathInRawDataDecoder = "Invalid 'path' in 'RawDataDecoder' decoder. Expected format: '$data.raw_log_data#3'.";

    /// <summary>
    /// Message indicating an error occurred while parsing decoder parameters.
    /// </summary>
    public const string ErrorParsingDecoderParams = "Error parsing decoder parameters for 'RawDataDecoder' or 'StaticDataDecoder'.";

    /// <summary>
    /// Format message indicating that a specific decoder type was not found.
    /// </summary>
    public const string DecoderTypeNotFound = "Decoder type {0} not found.";

    /// <summary>
    /// General message indicating an issue with retrieving data from Covalent.
    /// </summary>
    public const string GeneralCovalentDataRetrievalError = "There was an issue retrieving data from Covalent. {0}";

    /// <summary>
    /// Message indicating that a class specification is missing or incorrect.
    /// </summary>
    public const string ClassSpecificationError = "The class is missing or incorrectly specified: {0}";

    /// <summary>
    /// Message template for an error indicating that the starting block is invalid because it is greater than the chain's current last block.
    /// </summary>
    public const string InvalidStartingBlockErrorMessage = "Invalid StartingBlock on chain {0}: Greater than the chain's current last block {1}.";

    /// <summary>
    /// Message template for logging when the EndingBlock value is adjusted.
    /// </summary>
    public const string EndingBlockLogMessage = "On chain = {0}, StartingBlock ({1}) was greater than EndingBlock ({2}), but this has been corrected.\n";
}