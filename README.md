# .NET Blazor Movie
[![Build Status](https://github.com/Thuyen21/Blazor-Movie-/actions/workflows/format.yml/badge.svg)](https://github.com/Thuyen21/Blazor-Movie-/actions/workflows/format.yml/badge.svg)

This project was built on .NET 6

## Getting Started ##

* [Install .NET 6](https://dotnet.microsoft.com/download/dotnet/6.0/)
* [.NET Blazor documentation](https://docs.microsoft.com/en-us/aspnet/core/blazor/?view=aspnetcore-6.0/)
* [.NET MAUI documentation](https://docs.microsoft.com/dotnet/maui)

## Current News ##

* September 25, 2021 - [.NET Blazor Movie RC 1.2](https://github.com/Thuyen21/Blazor-Movie-/tree/rc.1.2)
* September 24, 2021 - [.NET Blazor Movie RC 1.1](https://github.com/Thuyen21/Blazor-Movie-/tree/RC1.1)
* September 18, 2021 - [.NET Blazor Movie RC 1](https://github.com/Thuyen21/Blazor-Movie-/tree/Rc1/)

## Contributor Guide ##

### Requirements ###

- [Install .NET6](https://dotnet.microsoft.com/download/dotnet/6.0)

### Running

#### .NET 6

##### Compile with globally installed `dotnet`

Install global workloads

```dotnetcli
dotnet workload install maui wasm-tools
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
