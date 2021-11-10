# .NET Blazor Movie

[![Build Status](https://github.com/Thuyen21/Blazor-Movie-/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Thuyen21/Blazor-Movie-/actions/workflows/dotnet.yml/badge.svg)

This project was built on .NET 6

## Getting Started

- [Install .NET 6](https://dotnet.microsoft.com/download/dotnet/6.0/)
- [.NET Blazor documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0/)
- [.NET MAUI documentation](https://docs.microsoft.com/dotnet/maui)

## Current News
- November 11, 2021 - [.NET Blazor Movie 1.0](https://github.com/Thuyen21/Blazor-Movie-/tree/1.1)
- November 9, 2021 - [.NET Blazor Movie 1.0](https://github.com/Thuyen21/Blazor-Movie-/tree/1.0)

## Contributor Guide

### Requirements

- [Install .NET6](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)

### Running

#### Visual Studio 2022

First thing’s first, install Visual Studio 2022 and install the .NET WebAssembly build tools, select the optional component in the Visual Studio installer.

For Maui development check .NET MAUI (preview) under the Mobile Development with .NET workload, and check the Universal Windows Platform development workload. Then, install the [Windows App SDK Single-project MSIX extension](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

#### .NET 6

##### Compile with globally installed `dotnet`

Clear Nuget 

```dotnetcli
dotnet nuget locals all --clear
```

Install global workloads

```dotnetcli
dotnet workload install android ios maccatalyst tvos macos maui wasm-tools
```

Build and launch Visual Studio using global workloads

```dotnetcli
dotnet tool restore BlazorMovie.sln
dotnet build BlazorMovie.sln
```

Release project

```dotnetcli
dotnet release BlazorMovie.sln
```

Run project

```dotnetcli
dotnet BlazorMovie.Server.dll
```
