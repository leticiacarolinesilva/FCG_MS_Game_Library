# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj", "src/UserRegistrationAndGameLibrary.Api/"]
COPY ["src/UserRegistrationAndGameLibrary.Application/UserRegistrationAndGameLibrary.Application.csproj", "src/UserRegistrationAndGameLibrary.Application/"]
COPY ["src/UserRegistrationAndGameLibrary.Domain/UserRegistrationAndGameLibrary.Domain.csproj", "src/UserRegistrationAndGameLibrary.Domain/"]
COPY ["src/UserRegistrationAndGameLibrary.Infra/UserRegistrationAndGameLibrary.Infra.csproj", "src/UserRegistrationAndGameLibrary.Infra/"]
COPY ["src/UserRegistrationAndGameLibrary.UnitTest/UserRegistrationAndGameLibrary.UnitTest.csproj", "src/UserRegistrationAndGameLibrary.UnitTest/"]

# Restore packages
RUN dotnet restore "src/UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj"
RUN dotnet restore "src/UserRegistrationAndGameLibrary.UnitTest/UserRegistrationAndGameLibrary.UnitTest.csproj"

# Copy the rest of the code
COPY . .

# Build the projects
RUN dotnet build "src/UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj" -c Release
RUN dotnet build "src/UserRegistrationAndGameLibrary.UnitTest/UserRegistrationAndGameLibrary.UnitTest.csproj" -c Release

# Test stage
FROM build AS test
WORKDIR /src
RUN dotnet test "src/UserRegistrationAndGameLibrary.UnitTest/UserRegistrationAndGameLibrary.UnitTest.csproj" -c Release --no-build
# Build and publish
FROM build AS publish
RUN dotnet build "src/UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj" -c Release -o /app/build
RUN dotnet publish "src/UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

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
ENTRYPOINT ["dotnet", "UserRegistrationAndGameLibrary.Api.dll"]
