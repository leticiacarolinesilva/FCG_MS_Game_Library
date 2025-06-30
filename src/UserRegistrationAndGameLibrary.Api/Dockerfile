FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj", "UserRegistrationAndGameLibrary.Api/"]
COPY ["UserRegistrationAndGameLibrary.Application/UserRegistrationAndGameLibrary.Application.csproj", "UserRegistrationAndGameLibrary.Application/"]
COPY ["UserRegistrationAndGameLibrary.Domain/UserRegistrationAndGameLibrary.Domain.csproj", "UserRegistrationAndGameLibrary.Domain/"]
COPY ["UserRegistrationAndGameLibrary.Infra/UserRegistrationAndGameLibrary.Infra.csproj", "UserRegistrationAndGameLibrary.Infra/"]

RUN dotnet restore "UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj"

COPY . .

WORKDIR "/src/UserRegistrationAndGameLibrary.Api"
RUN dotnet build "UserRegistrationAndGameLibrary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserRegistrationAndGameLibrary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserRegistrationAndGameLibrary.Api.dll"]
