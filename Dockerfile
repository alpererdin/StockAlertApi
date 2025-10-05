FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/StockAlertApi.API/StockAlertApi.API.csproj", "src/StockAlertApi.API/"]
COPY ["src/StockAlertApi.Application/StockAlertApi.Application.csproj", "src/StockAlertApi.Application/"]
COPY ["src/StockAlertApi.Core/StockAlertApi.Core.csproj", "src/StockAlertApi.Core/"]
COPY ["src/StockAlertApi.Infrastructure/StockAlertApi.Infrastructure.csproj", "src/StockAlertApi.Infrastructure/"]
RUN dotnet restore "src/StockAlertApi.API/StockAlertApi.API.csproj"
COPY . .
WORKDIR "/src/src/StockAlertApi.API"
RUN dotnet build "StockAlertApi.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StockAlertApi.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StockAlertApi.API.dll"]