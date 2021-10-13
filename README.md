# .NET Blazor Movie

[![Build Status](https://github.com/Thuyen21/Blazor-Movie-/actions/workflows/format.yml/badge.svg)](https://github.com/Thuyen21/Blazor-Movie-/actions/workflows/format.yml/badge.svg)

This project was built on .NET 6

## Getting Started

- [Install .NET 6](https://dotnet.microsoft.com/download/dotnet/6.0/)
- [.NET Blazor documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0/)
- [.NET MAUI documentation](https://docs.microsoft.com/dotnet/maui)

## Current News

- October 13, 2021 - [.NET Blazor Movie RC.2.0](https://github.com/Thuyen21/Blazor-Movie-/tree/RC.2.0)
- October 10, 2021 - [.NET Blazor Movie RC.2.0 Preview](https://github.com/Thuyen21/Blazor-Movie-/tree/RC.2.0-Preview)
- October 1, 2021 - [.NET Blazor Movie RC 1.5](https://github.com/Thuyen21/Blazor-Movie-/tree/Version)
- September 29, 2021 - [.NET Blazor Movie RC 1.4 UI Update](https://github.com/Thuyen21/Blazor-Movie-/tree/baopngch18183/UI-rc1.4)
- September 28, 2021 - [.NET Blazor Movie RC 1.3](https://github.com/Thuyen21/Blazor-Movie-/tree/RC_1.3)
- September 25, 2021 - [.NET Blazor Movie RC 1.2](https://github.com/Thuyen21/Blazor-Movie-/tree/rc.1.2)
- September 24, 2021 - [.NET Blazor Movie RC 1.1](https://github.com/Thuyen21/Blazor-Movie-/tree/RC1.1)
- September 18, 2021 - [.NET Blazor Movie RC 1](https://github.com/Thuyen21/Blazor-Movie-/tree/Rc1/)

## Contributor Guide

### Requirements

- [Install .NET6](https://dotnet.microsoft.com/download/dotnet/6.0)
- [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)

### Running

#### Visual Studio 2022

First thing’s first, install Visual Studio 2022 and install the .NET WebAssembly build tools, select the optional component in the Visual Studio installer, and check .NET MAUI (preview) under the Mobile Development with .NET workload, and check the Universal Windows Platform development workload.

Now, install the [Windows App SDK Single-project MSIX extension](https://marketplace.visualstudio.com/items?itemName=ProjectReunion.MicrosoftSingleProjectMSIXPackagingToolsDev17)

#### .NET 6

##### Compile with globally installed `dotnet`

Install global workloads

```dotnetcli
dotnet workload install android ios maccatalyst tvos macos maui wasm-tools
```

Build and launch Visual Studio using global workloads

```dotnetcli
dotnet tool restore
dotnet build BlazorApp3.sln
```

Release project

```dotnetcli
dotnet release BlazorApp3.sln
```

Run project

```dotnetcli
dotnet BlazorApp3.Server.dll
```
