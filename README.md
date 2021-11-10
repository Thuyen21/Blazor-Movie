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
![Untitled](https://user-images.githubusercontent.com/65522631/141151945-2180827c-a9d7-4cdb-976c-e94ae05ae391.png)


For Maui development check .NET MAUI (preview) under the Mobile Development with .NET workload, and check the Universal Windows Platform development workload. Then, install the [Windows App SDK Single-project MSIX extension](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)
![Untitled](https://user-images.githubusercontent.com/65522631/141134629-db279f1b-510c-4739-8fac-0a0ab38247fb.png)

For Maui Run

![Screenshot 2021-11-10 215220](https://user-images.githubusercontent.com/65522631/141135723-4ae08096-20cb-47f8-b85f-8a0f43cf3933.png)

Choose Window, Android, macOS, IOS

![Screenshot 2021-11-10 215407](https://user-images.githubusercontent.com/65522631/141136014-5b7d58d4-b354-41ca-85af-2a1a7def3d56.png)

For Blazor Run

![Screenshot 2021-11-10 215328](https://user-images.githubusercontent.com/65522631/141135873-7ecd3d4b-a51c-497f-b053-f3bde979e48d.png)

#### .NET 6 Cli user

##### Compile with globally installed `dotnet`

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
