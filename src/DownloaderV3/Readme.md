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

## Dependency Injection Setup Setup
To use DownloaderV3, set up services using `ServiceConfigurator` by default with context and logger configured by default:

```csharp
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Helpers;
using DownloaderV3.Destination;

// Create the context for your database
var context = new CustomDestination(new DbContextOptionsBuilder<CustomDestination>()
    .UseSqlServer("your-connection-string")
    .Options);

// Set up services with context and logging (using console logging by default)
var downloadHandler = new DownloadHandler<InputData>(context);

```

`ServiceConfigurator` helps in setting up required dependencies, such as logging, database context, and factories, using default configurations when necessary.

## Key Classes
- **DownloadHandler<TData>**: The main handler for fetching, decoding, and saving blockchain data.
- **BaseDestination**: Extend this class to define your own destination for data storage.

## Dependency Injection Setup (Second Option)

```csharp
using Microsoft.Extensions.DependencyInjection;
using DownloaderV3.Helpers;
using DownloaderV3.Destination;
using Microsoft.Extensions.Logging;

// Set up your services
var services = new ServiceCollection();

// Configure the context and logging
services.AddDbContext<CustomDestination>(options =>
{
    options.UseSqlServer("your-connection-string");
});

services.AddLogging(config => config.AddConsole());

// Configure other services like GetSourcePage, IDocumentFactory, etc.
services.AddTransient<GetSourcePage, MyCustomSourcePage>();
services.AddTransient<IDocumentFactory, MyCustomDocumentFactory>();
services.AddTransient<IDocumentDecoderFactory, MyCustomDocumentDecoderFactory>();

// Build the service provider
var serviceProvider = services.BuildServiceProvider();

// Use the DownloadHandler with the service provider
var downloadHandler = new DownloadHandler<InputData>(serviceProvider);

// Use the handler
var results = await downloadHandler.HandleAsync();
```

## Advanced Configuration
To create a custom destination, extend `BaseDestination`:
```csharp
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