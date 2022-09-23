#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

COPY . .

CMD dotnet dev-certs https --trust
CMD ASPNETCORE_URLS=http://*:$PORT dotnet BlazorMovie.Server.dll
