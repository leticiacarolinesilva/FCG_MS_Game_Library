# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# Base image para execução
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagem para build da aplicação
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copia os arquivos .csproj
COPY ["UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj", "UserRegistrationAndGameLibrary.Api/"]
COPY ["UserRegistrationAndGameLibrary.Application/UserRegistrationAndGameLibrary.Application.csproj", "UserRegistrationAndGameLibrary.Application/"]
COPY ["UserRegistrationAndGameLibrary.Domain/UserRegistrationAndGameLibrary.Domain.csproj", "UserRegistrationAndGameLibrary.Domain/"]
COPY ["UserRegistrationAndGameLibrary.Infra/UserRegistrationAndGameLibrary.Infra.csproj", "UserRegistrationAndGameLibrary.Infra/"]

# Restaura os pacotes NuGet
RUN dotnet restore "UserRegistrationAndGameLibrary.Api/UserRegistrationAndGameLibrary.Api.csproj"

# Copia todos os arquivos
COPY . .

# Compila o projeto
WORKDIR "/src/UserRegistrationAndGameLibrary.Api"
RUN dotnet build "UserRegistrationAndGameLibrary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publica o projeto para produção
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "UserRegistrationAndGameLibrary.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagem final de runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserRegistrationAndGameLibrary.Api.dll"]
