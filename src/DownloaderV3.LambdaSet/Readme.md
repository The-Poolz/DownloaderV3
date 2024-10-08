# DownloaderV3.LambdaSet

**DownloaderV3.LambdaSet** is an AWS Lambda function implementation that serves as a core component of the **DownloaderV3** system. This function is designed to automate the downloading, processing, and storing of data efficiently, leveraging the serverless capabilities of AWS Lambda. By utilizing AWS services, the function ensures seamless scalability, allowing the system to dynamically adjust to varying workloads and data volumes without manual intervention. The integration with AWS not only optimizes performance but also provides a reliable and robust mechanism for real-time data management from source to destination.

---
## Environment Variables
To ensure proper functionality, the following environment variables must be configured for the Lambda function:

- **CONFIGUREDSQLCONNECTION_SECRET_NAME_OF_CONNECTION**: Specifies the name of the secret that contains the database connection string. Example value: "Downloader".
- **LastBlockDownloaderUrl**: URL to fetch the latest block status from Covalent API. Example value: "https://api.covalenthq.com/...".
- **LastBlockKey**: API key used for authentication with the Covalent API. Example value: "cqt_**********".
- **ApiUrl**: Template URL for accessing token holders from the Covalent API. Example value: "https://api.covalenthq.com/v1/....".

### Environment Variables Configuration Example:

```json
"environment-variables": "\"CONFIGUREDSQLCONNECTION_SECRET_NAME_OF_CONNECTION\"=\"Downloader\";\"LastBlockDownloaderUrl\"=\"https://api.covalenthq.com/...\";\"LastBlockKey\"=\"cqt_**********\";\"ApiUrl\"=\"https://api.covalenthq.com/v1/....\";"

```

## Project Structure

The project consists of the following components:

- LambdaFunction.cs — The main file containing the AWS Lambda function handler method, where the core logic of the function resides.
- aws-lambda-tools-defaults.json — A configuration file with default settings for deployment using Visual Studio and AWS deployment tools.

## System Overview

The **DownloaderV3** system is designed to manage the downloading, processing, and storage of data from various sources and destinations, leveraging the modular architecture provided by AWS Lambda. The system is built with a focus on scalability and efficiency, ensuring that each component is optimized for performance in a serverless environment.

### Base Packages Used
The project uses two main packages as dependencies:

1. **DownloaderV3** (Version 1.0.0)
```xml
<PackageReference Include="DownloaderV3" Version="1.0.0" />
```
- **Description**: This package contains the core logic for the DownloaderV3 system, specifically focused on handling data from sources such as Covalent. It includes tools and utilities for extracting, transforming, and managing data in a distributed manner.
- **Namespace**: DownloaderV3.Source.CovalentDocument
- **Functionality**: It provides methods and classes necessary for interfacing with Covalent, setting up document processing pipelines, and managing data transformation operations required by the system.

2. **DownloaderV3.DataBase** (Version 1.0.0)
```xml
<PackageReference Include="Amazon.Lambda.Core" Version="1.1.0" />
```
- **Description**: This package serves as the destination module for the DownloaderV3 system, focusing on handling the storage and database operations required to persist the downloaded and processed data.
- **Namespace**: DownloaderV3.Destination
- **Functionality**: It integrates with the database, managing the schema and entities for the stored data. This package ensures that the processed data is efficiently and accurately saved in the database, supporting scalability and maintainability.

## How the System Works
- **Data Flow**: The **DownloaderV3** Lambda function processes data by extracting information from the source (Covalent) using the **DownloaderV3** package and stores it in the database using the **DownloaderV3.DataBase** package. This setup ensures a seamless flow of data from source to destination.
- **Modular Design**: By separating the logic into source (`DownloaderV3.Source.CovalentDocument`) and destination (`DownloaderV3.Destination`), the system maintains flexibility and allows for easy extension or modification of either module without affecting the other.
- **AWS Integration**: The system is tightly integrated with AWS services like Lambda, enabling it to automatically scale and download data efficiently from the source (Covalent) as new events or triggers occur. This automation ensures that the system can handle various data loads dynamically and efficiently without requiring manual intervention, maintaining real-time data processing and accuracy.

## Additional Information
- **Deployment**: Follow the steps in Visual Studio to deploy the Lambda function and configure necessary event sources (e.g., AWS Secrets Manager).
- **Monitoring**: Utilize AWS CloudWatch logs and metrics to monitor the function’s performance and troubleshoot any issues that arise during execution.
- **Testing**: Make use of Visual Studio’s testing tools or set up a unit testing project to validate the behavior and performance of your Lambda function before deployment.