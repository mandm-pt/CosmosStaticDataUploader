# Cosmos Static Data Uploader
[![Build status](https://ci.appveyor.com/api/projects/status/xkd82di0v4tq3xfy?svg=true)](https://ci.appveyor.com/project/mandm-pt/cosmosstaticdatauploader)

Basic tool that uploads multiple object files into Azure Cosmos database.

# Usage

```
Usage: workingDir Env

    workingDir - Folder that contains the documents to upload.
    Env - Environment. Supported values are dependent on settings defined on appsettings.json file.
```

# Example

```
C:\>dotnet .\CosmosStaticDataUploader.dll .\Data\ local
```
