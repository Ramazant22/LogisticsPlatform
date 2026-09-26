# 1. Aşama: Derleme (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Önce proje dosyalarını kopyala (Cache optimizasyonu için)
COPY ["src/API/LogisticsPlatform.API.csproj", "src/API/"]
COPY ["src/BuildingBlocks/LogisticsPlatform.BuildingBlocks.csproj", "src/BuildingBlocks/"]
COPY ["src/Modules/Fleet/LogisticsPlatform.Modules.Fleet.csproj", "src/Modules/Fleet/"]
COPY ["src/Modules/Identity/LogisticsPlatform.Modules.Identity.csproj", "src/Modules/Identity/"]
COPY ["src/Modules/Tenancy/LogisticsPlatform.Modules.Tenancy.csproj", "src/Modules/Tenancy/"]
COPY ["src/Modules/Shipment/LogisticsPlatform.Modules.Shipment.csproj", "src/Modules/Shipment/"]
COPY ["src/Modules/Drivers/LogisticsPlatform.Modules.Drivers.csproj", "src/Modules/Drivers/"]
COPY ["src/Modules/Customers/LogisticsPlatform.Modules.Customers.csproj", "src/Modules/Customers/"]
COPY ["src/Modules/Orders/LogisticsPlatform.Modules.Orders.csproj", "src/Modules/Orders/"]
COPY ["src/Modules/Tracking/LogisticsPlatform.Modules.Tracking.csproj", "src/Modules/Tracking/"]
COPY ["src/Modules/Billing/LogisticsPlatform.Modules.Billing.csproj", "src/Modules/Billing/"]
COPY ["src/Modules/Notifications/LogisticsPlatform.Modules.Notifications.csproj", "src/Modules/Notifications/"]
COPY ["src/Modules/Routes/LogisticsPlatform.Modules.Routes.csproj", "src/Modules/Routes/"]
COPY ["src/Modules/Reporting/LogisticsPlatform.Modules.Reporting.csproj", "src/Modules/Reporting/"]

# Paketleri geri yükle (Restore)
RUN dotnet restore "src/API/LogisticsPlatform.API.csproj"

# Tüm kaynak kodları kopyala ve projeyi derle (Publish)
COPY . .
WORKDIR "/src/src/API"
RUN dotnet publish "LogisticsPlatform.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 2. Aşama: Çalıştırma (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080

# Derlenmiş dosyaları build aşamasından al
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "LogisticsPlatform.API.dll"]
