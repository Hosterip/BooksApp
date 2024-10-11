# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/BooksApp.API/BooksApp.API.csproj", "src/BooksApp.API/"]
COPY ["src/BooksApp.Application/BooksApp.Application.csproj", "src/BooksApp.Application/"]
COPY ["src/BooksApp.Domain/BooksApp.Domain.csproj", "src/BooksApp.Domain/"]
COPY ["src/BooksApp.Infrastructure/BooksApp.Infrastructure.csproj", "src/BooksApp.Infrastructure/"]
RUN dotnet restore "./src/BooksApp.API/BooksApp.API.csproj"
COPY . .
WORKDIR "/src/src/BooksApp.API"
RUN dotnet build "./BooksApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BooksApp.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BooksApp.API.dll"]