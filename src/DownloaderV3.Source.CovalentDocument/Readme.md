# DownloaderV3.Source.CovalentDocument

`DownloaderV3.Source.CovalentDocument` defines a set of classes that manage the creation, downloading, and decoding of event data using Covalent's API. This module is part of the DownloaderV3 system and works in conjunction with `DownloaderV3.Destination` and other components.

The core responsibilities of this module include fetching event logs, serializing them, decoding the data, and saving it to the destination database. This module makes heavy use of the factory pattern, making it flexible and extensible for different implementations of downloading and decoding event data.

## Features
- Event Data Fetching: Fetch event logs from Covalent API.
- Data Decoding: Decode and process blockchain event data.
- Factory Pattern: Easily replace implementations of document fetching and decoding through factories (`IDocumentFactory` and `IDocumentDecoderFactory`).
- Pagination Handling: Manage event data pagination and retrieval from smart contracts.

## Key Classes
### 1. `CovalentDocument` (via Factory)
The `CovalentDocument` class is responsible for:

- Fetching event logs from the Covalent API.
- Serializing the downloaded event data into a structured format (`InputData`).

The `CovalentDocument` is instantiated through a factory (`IDocumentFactory`), allowing for flexible implementation changes. It extends from `BaseDocument<TData>` and manages pagination and retrieval of event logs for smart contracts.

```csharp
var document = _documentFactory.Create<InputData>(pageNumber, downloaderSettings, lastBlockDictionary, chainSettings);
````

The factory ensures that different implementations of `BaseDocument<TData>` can be provided without changing the core logic.

### 2. DocumentDecoder (via Factory)
The `DocumentDecoder` class is responsible for:

- Decoding the serialized event data fetched by `CovalentDocument`.
- Saving the decoded data to the destination storage.

The `DocumentDecoder` is created via `IDocumentDecoderFactory`. Like `CovalentDocument`, this class can be replaced or extended with a custom implementation, making it adaptable to new event types or blockchain data formats. It extends from `BaseDocumentDecoder<InputData>` and processes event data by extracting relevant fields and saving them into the appropriate destination.

Example:
```csharp
var documentDecoder = _documentDecoderFactory.Create(downloaderSettings, document.DownloadedContractData!);
documentDecoder.DocumentResponses.LockedSaveAll(_destination);
```

### 3. Factory Pattern and Custom Implementations
Both `CovalentDocument` and `DocumentDecoder` are created using factory patterns, which makes it easy to switch out implementations. This pattern provides the flexibility to support various APIs or data sources without altering the core structure.

## Getting Started
### Prerequisites
- .NET 8.0 or later
- Access to Covalent's API URL with an API key

## Installation
Clone the repository and add it to your solution:

```csharp
git clone https://github.com/The-Poolz/DownloaderV3.git
```

Or, if you've packaged it as a NuGet package:

```csharp
dotnet add package DownloaderV3.Source.CovalentDocument
```

## Configuration
Before using the `CovalentDocument` or `GetLastBlockCovalent`, make sure to configure your environment with the appropriate API URL and API key.

#### Example of setting up environment variables:
```csharp
export ApiUrl="https://api.covalenthq.com/"
export ApiKey="your_covalent_api_key"
```

## Example Usage
Here is an example of how to fetch and decode event data using the `CovalentDocument` and `DocumentDecoder`:

```csharpusing DownloaderV3.Source.CovalentDocument;
using DownloaderV3.Source.CovalentDocument.Document;
using DownloaderV3.Source.CovalentDocument.Document.DocumentDecoder;

var document = _documentFactory.Create<InputData>(pageNumber, downloaderSettings, lastBlockDictionary, chainSettings);
var documentDecoder = _documentDecoderFactory.Create(downloaderSettings, document.DownloadedContractData!);

documentDecoder.DocumentResponses.LockedSaveAll(_destination);

```

## Contributing
We welcome contributions! Please feel free to submit issues, fork the repository, and send pull requests.

## License
This project is licensed under the MIT License. For more details, see the LICENSE file.