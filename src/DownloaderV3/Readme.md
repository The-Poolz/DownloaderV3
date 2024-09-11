# DownloaderV3 Project

DownloaderV3 is a flexible and reusable .NET library designed for downloading and processing data using a provided database context. The library is intended to be integrated into various projects that require reliable data handling operations.

## Features
- Extensible: Easily integrate with different database contexts by extending the base context (BaseDestination).
- Simple API: Provides an easy-to-use API for initializing and running data download processes.
- Modular Design: Designed to be modular and easily extendable to fit different data processing needs.

## Getting Started
Prerequisites
.NET 8.0 or later
An existing project where you want to integrate the DownloaderV3 library

## Installation
Clone the repository and add it to your solution:

```csharp
git clone https://github.com/The-Poolz/DownloaderV3.git
```

Or, if you've packaged it as a NuGet package:

```csharp
dotnet add package DownloaderV3
```


## Usage
1. Add a Reference: Add a reference to the DownloaderV3 project in your solution.

2. Instantiate the DownloadHandler class: Pass your specific BaseDestination or a derived context.

Example:

```csharp
using DownloaderV3;
using DownloaderV3.Destination;

var destination = new YourDerivedDestination(/* options */);
var downloader = new DownloadHandler(destination, sourcePage);
var results = await downloader.HandleAsync();
```
3. Configure Database Context: Ensure your database context (e.g., DownloaderV3.Destination) is correctly configured and passed to the library.

## Source Integration:
For using DownloaderV3.Source.CovalentLastBlock:

```
using DownloaderV3.Source.CovalentLastBlock;

var source = new GetLastBlockCovalent();
var data = source.FetchData();
```

## Contributing
Contributions are welcome! Please feel free to submit issues, fork the repository, and send pull requests.

## License
This project is licensed under the MIT License - see the LICENSE file for details.