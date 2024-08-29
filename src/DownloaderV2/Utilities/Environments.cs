﻿using EnvironmentManager.Attributes;

namespace DownloaderV2.Utilities
{
    /// <summary>
    /// Enum for environment variable keys.
    /// </summary>
    public enum Environments
    {
        [EnvironmentVariable(isRequired: true)]
        LastBlockKey,

        [EnvironmentVariable(isRequired: true)]
        LastBlockDownloaderUrl,

        [EnvironmentVariable(isRequired: true)]
        ApiUrl
    }
}