namespace DownloaderV3.Source.CovalentDocument.Helpers;

/// <summary>
/// Provides a collection of exception message templates.
/// </summary>
public static class ExceptionMessages
{
    /// <summary>
    /// Message template for an error indicating that the starting block is invalid because it is greater than the chain's current last block.
    /// </summary>
    public const string InvalidStartingBlockErrorMessage = "Invalid StartingBlock on chain {0}: Greater than the chain's current last block {1}.";
}