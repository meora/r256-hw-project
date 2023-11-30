FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Ozon.Route256.Practice.OrderService/*.csproj", "Ozon.Route256.Practice.OrderService/"]
COPY ["Ozon.Route256.Practice.OrderService/Protos", "Ozon.Route256.Practice.OrderService/Protos/"]
COPY ["Ozon.Route256.Practice.OrderService.Application/*.csproj", "Ozon.Route256.Practice.OrderService.Application/"]
COPY ["Ozon.Route256.Practice.OrderService.Infrastructure/*.csproj", "Ozon.Route256.Practice.OrderService.Infrastructure/"]
COPY ["Ozon.Route256.Practice.OrderService.Infrastructure/Protos", "Ozon.Route256.Practice.OrderService.Infrastructure/Protos/"]
COPY ["Ozon.Route256.Practice.OrderService.Domain/*.csproj", "Ozon.Route256.Practice.OrderService.Domain/"]
RUN dotnet restore "Ozon.Route256.Practice.OrderService/Ozon.Route256.Practice.OrderService.csproj"
COPY . .
WORKDIR "Ozon.Route256.Practice.OrderService"
RUN dotnet build "Ozon.Route256.Practice.OrderService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ozon.Route256.Practice.OrderService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Practice.OrderService.dll"]