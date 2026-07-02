# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project file and restore
COPY src/FootballStats.Api/FootballStats.Api.csproj src/FootballStats.Api/
RUN dotnet restore src/FootballStats.Api/FootballStats.Api.csproj

# Copy source and publish
COPY src/FootballStats.Api/ src/FootballStats.Api/
RUN dotnet publish src/FootballStats.Api/FootballStats.Api.csproj -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render assigns PORT dynamically
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "FootballStats.Api.dll"]
