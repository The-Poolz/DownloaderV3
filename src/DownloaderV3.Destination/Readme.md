# DownloaderV3.Destination

DownloaderV3.Destination is the core module responsible for managing the database context that stores and processes downloaded data within the DownloaderV3 system. This component is built on top of Entity Framework Core and provides a flexible structure for managing blockchain-related data. While the current implementation uses a database context, this may evolve in the future to accommodate other storage mechanisms or contexts.

## Features
- Extensible Database Context: The BaseDestination class acts as the foundation for managing data across different blockchain networks. It can be extended for custom use cases.
- Entity Management: Includes several key models (Chain, ChainInfo, DownloaderSettings, DownloaderMapping) that structure the blockchain data and downloading settings.
- EF Core Integration: Fully integrated with Entity Framework Core, allowing for robust database operations.
- Configurable: Easily configure database entities and extend them as needed for specific data processing requirements.

## Getting Started
- .NET 8.0 or later
- Entity Framework Core 7.0 or later

## Installation
Clone the repository and add it to your solution:

```csharp
git clone https://github.com/The-Poolz/DownloaderV3.git
```

Or, if you've packaged it as a NuGet package:

```csharp
dotnet add package DownloaderV3.Destination
```


## Usage
### BaseDestination
BaseDestination is the foundation database context that stores blockchain network data, downloading settings, and event mappings. It is not intended to be used directly but should be extended to introduce custom logic and database configurations that fit specific events or use cases.

You can create your own derived context from BaseDestination and customize it as needed.

### Key Entities
Chain: Stores information about different blockchain networks (ChainId, Name, RPC Connection).
ChainInfo: Holds specific details about the blockchain (block speed, warning time, etc.).
DownloaderSettings: Manages configuration for each download job (e.g., contract address, event hash, starting block, etc.).
DownloaderMapping: Defines the mapping of events and how to decode and process them from blockchain logs.

### Event Models with [ResponseModel]
When integrating event-specific data, models for these events must be marked with the [ResponseModel] attribute. This allows the system to recognize and process the event data correctly.

```csharp
using DownloaderV3.Destination.Models;

[ResponseModel]
public class SwapParty : Base
{
    public string TxHash { get; set; } = null!;
    public long BlockHeight { get; set; }
    public string SenderAddress { get; set; } = null!;
    public decimal Amount { get; set; }
}
```

### Adding the Event Model to Your Destination
After creating your event model, you'll need to update your custom destination context to include the new model.

Example: Creating an Event Model
To create a model that captures event data (e.g., SwapParty event), you need to define the class with the [ResponseModel] attribute and inherit from the base class Base.

```csharp
public class CustomDestination : BaseDestination
{
    public CustomDestination(DbContextOptions<CustomDestination> options)
        : base(options) { }

    // Adding the SwapParty event model
    public DbSet<SwapParty> SwapParties { get; set; } = null!;
}
```

### Configuring the Event in the Database
For the event to be captured and processed, it must be added to the DownloaderSettings in the database, specifying details such as the event hash and response model.

Example:
```json
{
  "ChainId": 56,
  "ContractAddress": "0x123...",
  "EventHash": "0xabc...",
  "ResponseType": "SwapParty",
  "StartingBlock": 1000000,
  "EndingBlock": 2000000,
  "Active": true,
  "UrlSet": "https://api.covalenthq.com",
  "MaxBatchSize": 10000,
  "MaxPageNumber": 1000,
  "Key": "1234567890"
}
```

### Example Code
1) Extending BaseDestination:

To use BaseDestination, you need to create a new context by extending it. Here’s an example of how to do that:

```csharp
using DownloaderV3.Destination;
using Microsoft.EntityFrameworkCore;

public class CustomDestination : BaseDestination
{
    public CustomDestination(DbContextOptions<CustomDestination> options)
        : base(options) { }

    // You can add more entities or override existing ones here
    public DbSet<CustomEntity> CustomEntities { get; set; } = null!;
}
```

2) Configure and Use Your Custom Context:

After extending BaseDestination, you can use the custom context in your application:

Example of Entity Configuration:


```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder); // Inherits BaseDestination configurations

    modelBuilder.Entity<CustomEntity>(entity =>
    {
        entity.HasKey(e => e.Id);
        entity.Property(e => e.Name).HasMaxLength(100);
        // Add your additional configurations here
    });
}

```

## Contributing
We welcome contributions! Please feel free to submit issues, fork the repository, and send pull requests.

## License
This project is licensed under the MIT License. For more information, please check the LICENSE file in the repository.
 
---
This version reflects that BaseDestination cannot be used directly and requires extension.