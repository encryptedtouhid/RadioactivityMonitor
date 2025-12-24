# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy solution and project files
COPY RadioactivityMonitor/RadioactivityMonitor.sln .
COPY RadioactivityMonitor/src/RadioactivityMonitor.Core/RadioactivityMonitor.Core.csproj src/RadioactivityMonitor.Core/
COPY RadioactivityMonitor/src/RadioactivityMonitor.App/RadioactivityMonitor.App.csproj src/RadioactivityMonitor.App/
COPY RadioactivityMonitor/tests/RadioactivityMonitor.Tests/RadioactivityMonitor.Tests.csproj tests/RadioactivityMonitor.Tests/

# Restore dependencies
RUN dotnet restore

# Copy source code
COPY RadioactivityMonitor/src/ src/
COPY RadioactivityMonitor/tests/ tests/

# Run tests
RUN dotnet test --no-restore --verbosity minimal

# Build and publish
RUN dotnet publish src/RadioactivityMonitor.App/RadioactivityMonitor.App.csproj -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:9.0 AS runtime
WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# Set entry point
ENTRYPOINT ["dotnet", "RadioactivityMonitor.App.dll"]
