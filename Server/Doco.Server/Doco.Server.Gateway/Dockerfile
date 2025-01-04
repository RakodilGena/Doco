FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Doco.Server.Gateway/Doco.Server.Gateway.csproj", "Doco.Server.Gateway/"]
RUN dotnet restore "Doco.Server.Gateway/Doco.Server.Gateway.csproj"
COPY . .
WORKDIR "/src/Doco.Server.Gateway"
RUN dotnet build "Doco.Server.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Doco.Server.Gateway.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doco.Server.Gateway.dll"]
