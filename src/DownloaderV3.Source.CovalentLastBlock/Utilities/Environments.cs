using EnvironmentManager.Attributes;

namespace DownloaderV3.Source.CovalentLastBlock.Utilities;

/// <summary>
/// Enum for environment variable keys.
/// </summary>
public enum Environments
{
    [EnvironmentVariable(isRequired: true)]
    LastBlockKey,

    [EnvironmentVariable(isRequired: true)]
    LastBlockDownloaderUrl,
}