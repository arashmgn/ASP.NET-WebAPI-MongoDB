FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/AuthService.Domain/AuthService.Domain.csproj", "src/AuthService.Domain/"]
COPY ["src/AuthService.Application/AuthService.Application.csproj", "src/AuthService.Application/"]
COPY ["src/AuthService.Infrastructure/AuthService.Infrastructure.csproj", "src/AuthService.Infrastructure/"]
COPY ["src/AuthService.Api/AuthService.Api.csproj", "src/AuthService.Api/"]
RUN dotnet restore "src/AuthService.Api/AuthService.Api.csproj"
COPY . .
RUN dotnet publish "src/AuthService.Api/AuthService.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AuthService.Api.dll"]
