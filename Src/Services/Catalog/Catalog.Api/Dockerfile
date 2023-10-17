#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MicroServiceSample.sln", "MicroServiceSample.sln"]
COPY ["BuildingBlocks/Logging/Common.Loggin.csproj", "BuildingBlocks/Logging/"]
COPY ["Services/Catalog/Catalog.Api/Catalog.Api.csproj", "Services/Catalog/Catalog.Api/"]
#RUN dotnet restore "Catalog.Api/Catalog.Api.csproj"
COPY "docker-compose.dcproj" "docker-compose.dcproj"
COPY "Nuget.config" "Nuget.config"
RUN dotnet restore "MicroServiceSample.sln"

COPY . .
WORKDIR "/src/Services/Catalog/Catalog.Api"
RUN dotnet publish --no-restore -c -Release -o /app
FROM build As publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]