# DownloaderV3

DownloaderV3 is a powerful and flexible .NET library designed to download, decode, and process blockchain event data from sources like Covalent API. The project can be used to fetch blockchain logs, decode them, and store them in the destination of your choice.

## Features 
- Modular design with support for multiple data sources and destination databases.
- Extensible architecture allowing easy integration of new sources or event decoders.
- Uses the factory pattern to create customizable documents and decoders.
- Integrated with Entity Framework Core for managing database contexts.

## Getting Started
### Prerequisites
- .NET 8.0 or later
- Entity Framework Core 7.0 or later
- Access to Covalent API or other blockchain data sources

## Installation
Clone the repository and add it to your solution:

```csharp
git clone https://github.com/The-Poolz/DownloaderV3.git
```

Or, if you've packaged it as a NuGet package:

```csharp
dotnet add package DownloaderV3
```

## Using Dependency Injection (Recommended)
The preferred way to use DownloaderV3 is to take advantage of .NET’s built-in dependency injection system (IServiceProvider). Here's how you can set it up:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DownloaderV3;
using DownloaderV3.Source.CovalentLastBlock;
using DownloaderV3.Destination;
using DownloaderV3.Source.CovalentDocument;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;
using Microsoft.EntityFrameworkCore;

var services = new ServiceCollection();

// Registering required services and dependencies
services.AddLogging(config => config.AddConsole());

// Registering database context
services.AddDbContext<BaseDestination>(options =>
{
    options.UseInMemoryDatabase("InMemoryDb");
}, ServiceLifetime.Scoped);

// Registering necessary factories and classes
services.AddScoped<GetSourcePage, GetLastBlockCovalent>();
services.AddScoped<IDocumentFactory, DocumentFactory>();
services.AddScoped<IDocumentDecoderFactory, DocumentDecoderFactory>();

// Registering the main handler
services.AddScoped(typeof(DownloadHandler<>));

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Create the DownloadHandler instance using dependency injection
var downloadHandler = serviceProvider.GetRequiredService<DownloadHandler<InputData>>();

// Start the download process
var results = await downloadHandler.HandleAsync();

// Output results
foreach (var result in results)
{
    Console.WriteLine(result.ToString());
}

```
## Explanation

- `ServiceCollection` **Setup:** This is where we register all the dependencies required for the `DownloadHandler`. This includes database contexts, factories, and the `DownloadHandler` itself.
- `BuildServiceProvider`: Builds the service provider, which resolves dependencies at runtime.
- `HandleAsync()`: Kicks off the downloading process and handles the blockchain data fetching and saving.

## Key Classes
- `DownloadHandler<TData>`: The main class that coordinates fetching, decoding, and saving data. It uses the `IDocumentFactory` to create documents, and the IDocumentDecoderFactory to process and save the data.

- `GetSourcePage`: An abstract class that defines how to fetch the last block. Implementations like `GetLastBlockCovalent` provide the actual logic for different data sources.

- `BaseDestination`: Manages the destination for storing the fetched data. This must be extended to create a custom destination that fits your specific data needs.

## Example of Custom Destination
You can extend `BaseDestination` to create a custom destination where your data will be saved:

```csharp
using DownloaderV3.Destination;
using Microsoft.EntityFrameworkCore;

public class CustomDestination : BaseDestination
{
    public CustomDestination(DbContextOptions<CustomDestination> options)
        : base(options) { }

    public DbSet<CustomEntity> CustomEntities { get; set; } = null!;
}
```

## Contributing
We welcome contributions! Please feel free to submit issues, fork the repository, and send pull requests.

## License
This project is licensed under the MIT License. See the LICENSE file for more details.