#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

COPY . .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet BlazorApp3.Client.dll
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BlazorApp3.Server.dll