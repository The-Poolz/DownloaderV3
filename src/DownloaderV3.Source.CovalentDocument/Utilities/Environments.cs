using EnvironmentManager.Attributes;

namespace DownloaderV3.Source.CovalentDocument.Utilities
{
    /// <summary>
    /// Enum for environment variable keys.
    /// </summary>
    public enum Environments
    {
        [EnvironmentVariable(isRequired: true)]
        ApiKey,

        [EnvironmentVariable(isRequired: true)]
        ApiUrl
    }
}