# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/FCG_MS_Game_Library.Api/FCG_MS_Game_Library.Api.csproj", "src/FCG_MS_Game_Library.Api/"]
COPY ["src/FCG_MS_Game_Library.Application/FCG_MS_Game_Library.Application.csproj", "src/FCG_MS_Game_Library.Application/"]
COPY ["src/FCG_MS_Game_Library.Domain/FCG_MS_Game_Library.Domain.csproj", "src/FCG_MS_Game_Library.Domain/"]
COPY ["src/FCG_MS_Game_Library.Infra/FCG_MS_Game_Library.Infra.csproj", "src/FCG_MS_Game_Library.Infra/"]
COPY ["src/FCG_MS_Game_Library.UnitTest/FCG_MS_Game_Library.UnitTest.csproj", "src/FCG_MS_Game_Library.UnitTest/"]

# Restore packages
RUN dotnet restore "src/FCG_MS_Game_Library.Api/FCG_MS_Game_Library.Api.csproj"
RUN dotnet restore "src/FCG_MS_Game_Library.UnitTest/FCG_MS_Game_Library.UnitTest.csproj"

# Copy the rest of the code
COPY . .

# Build the projects
RUN dotnet build "src/FCG_MS_Game_Library.Api/FCG_MS_Game_Library.Api.csproj" -c Release
RUN dotnet build "src/FCG_MS_Game_Library.UnitTest/FCG_MS_Game_Library.UnitTest.csproj" -c Release

# Test stage
FROM build AS test
WORKDIR /src
RUN dotnet test "src/FCG_MS_Game_Library.UnitTest/FCG_MS_Game_Library.UnitTest.csproj" -c Release --no-build
# Build and publish
FROM build AS publish
RUN dotnet build "src/FCG_MS_Game_Library.Api/FCG_MS_Game_Library.Api.csproj" -c Release -o /app/build
RUN dotnet publish "src/FCG_MS_Game_Library.Api/FCG_MS_Game_Library.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y 'newrelic-dotnet-agent' \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-dotnet-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-dotnet-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY=475b169c910fd351f2d9da9b829ef653FFFFNRAL \
NEW_RELIC_APP_NAME="gamelibrary"


# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Copy the published app
COPY --from=publish /app/publish .

# Create a non-root user
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

# Expose port 80
EXPOSE 80

# Set the entry point
ENTRYPOINT ["dotnet", "FCG_MS_Game_Library.Api.dll"]
