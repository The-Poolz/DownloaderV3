using EnvironmentManager.Attributes;

namespace DownloaderV3.Source.CovalentDocument.Utilities
{
    /// <summary>
    /// Enum for environment variable keys.
    /// </summary>
    public enum Environments
    {
        [EnvironmentVariable(isRequired: true)]
        LastBlockKey,

        [EnvironmentVariable(isRequired: true)]
        ApiUrl
    }
}