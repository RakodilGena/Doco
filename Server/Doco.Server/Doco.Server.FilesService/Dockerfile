FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Doco.Server.FileService/Doco.Server.FileService.csproj", "Doco.Server.FileService/"]
RUN dotnet restore "Doco.Server.FileService/Doco.Server.FileService.csproj"
COPY . .
WORKDIR "/src/Doco.Server.FileService"
RUN dotnet build "Doco.Server.FileService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Doco.Server.FileService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Doco.Server.FileService.dll"]
