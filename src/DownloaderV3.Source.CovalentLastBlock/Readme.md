# DownloaderV3.Source.CovalentLastBlock

DownloaderV3.Source.CovalentLastBlock is a module designed to fetch the latest block data from the Covalent API. It provides functionality for downloading the most recent blockchain block information, which is essential for synchronizing with various blockchain networks and ensuring data consistency in the DownloaderV3 system.

## Features
- Covalent API Integration: Easily fetch the latest block heights from different blockchain networks using the Covalent API.
- Flexible Block Data Parsing: Parse block data from multiple networks (such as Ethereum, Binance Smart Chain, Arbitrum) into a unified format.
- Error Handling: Built-in error handling mechanisms for invalid or failed API responses.
- Extensible Design: Allows you to extend and customize the block-fetching logic by implementing new sources.

## Getting Started
### Prerequisites
- .NET 8.0 or later
- Covalent API key
- Covalent API URL for block data

## Installation
Clone the repository and add it to your solution:

```csharp
git clone https://github.com/The-Poolz/DownloaderV3.git
```

Or, if you've packaged it as a NuGet package:

```csharp
dotnet add package DownloaderV3.Source.CovalentLastBlock
```

## Configuration
Before using the GetLastBlockCovalent class, make sure to configure your environment with the appropriate API URL and key.

Example of setting up environment variables:
```csharp
export LastBlockDownloaderUrl="https://api.covalenthq.com/v1/chains/status/?key="
export LastBlockKey="your_covalent_api_key"
```

## Usage
DownloaderV3.Source.CovalentLastBlock provides a simple way to retrieve the latest block data from the Covalent API. The core class GetLastBlockCovalent is responsible for sending requests to the API and processing the response.

### Example Code
Fetching the Latest Block Data
Here’s an example of how to use GetLastBlockCovalent to fetch and parse the latest block data from the Covalent API:

```csharp
using DownloaderV3.Source.CovalentLastBlock;

var lastBlockSource = new LastBlockSource(_getSourcePage).LastBlockDictionary;

Console.WriteLine("Latest Block Data:");
foreach (var (chainId, blockHeight) in lastBlockSource)
{
    Console.WriteLine($"ChainId: {chainId}, BlockHeight: {blockHeight}");
}
```

## Class Details
### GetLastBlockCovalent
GetLastBlockCovalent is the default implementation that fetches the latest block data from the Covalent API. This class extends the abstract class GetSourcePage, which defines the core functionality for fetching and parsing data. GetLastBlockCovalent provides a specific implementation for interacting with the Covalent API.

### Custom Sources
You can implement additional sources by creating new classes that extend GetSourcePage. This allows you to integrate other APIs or data providers into the DownloaderV3 system.

Example API Response
The Covalent API returns the latest block data for multiple chains. Here's a sample response:

```json
{
    "data": {
        "updated_at": "2023-05-01T10:21:30Z",
        "items": [
            {
                "chain_id": 1,
                "synced_block_height": 17165351
            },
            {
                "chain_id": 56,
                "synced_block_height": 27826734
            },
            {
                "chain_id": 42161,
                "synced_block_height": 86156198
            }
        ]
    },
    "error": false
}
```

## Handling API Errors
If the API response contains an error or invalid data, the GetLastBlockCovalent class will throw an InvalidOperationException. You can catch and handle this exception as needed in your application.

## Contributing
We welcome contributions! Please feel free to submit issues, fork the repository, and send pull requests.

## License
This project is licensed under the MIT License. For more information, please check the LICENSE file in the repository.

---
This README provides information about the DownloaderV3.Source.CovalentLastBlock module, including class details, how to extend it for new sources, and usage instructions for fetching data from the Covalent API.