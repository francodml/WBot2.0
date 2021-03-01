#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:5.0-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["WBot2.csproj", ""]
RUN dotnet restore "./WBot2.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "WBot2.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WBot2.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WBot2.dll"]