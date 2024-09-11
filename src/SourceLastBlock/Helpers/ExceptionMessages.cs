namespace SourceLastBlock.Helpers;

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
    /// General message indicating an issue with retrieving data from Covalent.
    /// </summary>
    public const string GeneralCovalentDataRetrievalError = "There was an issue retrieving data from Covalent. {0}";
}