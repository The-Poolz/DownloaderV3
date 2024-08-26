using EnvironmentManager.Core;

namespace DownloaderV2.Utilities;

/// <summary>
/// Provides access to environment variables.
/// </summary>
public static class EnvironmentExtension
{
    private static readonly EnvManager EnvManager = new();

    /// <summary>
    /// Gets the value of the specified environment variable.
    /// </summary>
    /// <param name="key">The key of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    public static string Get(this Environments key) =>
        EnvManager.Get<string>(key.ToString());

    /// <summary>
    /// Gets the value of the specified environment variable with a specified type.
    /// </summary>
    /// <typeparam name="T">The type of the environment variable.</typeparam>
    /// <param name="key">The key of the environment variable.</param>
    /// <returns>The value of the environment variable.</returns>
    public static T Get<T>(this Environments key) =>
        EnvManager.Get<T>(key.ToString());
}