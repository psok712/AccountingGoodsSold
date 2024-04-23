FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["src/Ozon.Route256.Kafka.OrderEventConsumer.Presentation/Ozon.Route256.Kafka.OrderEventConsumer.Presentation.csproj", "src/Ozon.Route256.Kafka.OrderEventConsumer.Presentation/"]
COPY ["src/Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure/Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure.csproj", "src/Ozon.Route256.Kafka.OrderEventConsumer.Infrastructure/"]
COPY ["src/Ozon.Route256.Kafka.OrderEventConsumer.Domain/Ozon.Route256.Kafka.OrderEventConsumer.Domain.csproj", "src/Ozon.Route256.Kafka.OrderEventConsumer.Domain/"]
COPY ["src/Ozon.Route256.Kafka.OrderEventConsumer.Utils/Ozon.Route256.Kafka.OrderEventConsumer.Utils.csproj", "src/Ozon.Route256.Kafka.OrderEventConsumer.Utils/"]
COPY ["src/Ozon.Route256.Kafka.OrderEventGenerator/Ozon.Route256.Kafka.OrderEventGenerator.csproj", "src/Ozon.Route256.Kafka.OrderEventGenerator/"]
RUN dotnet restore "src/Ozon.Route256.Kafka.OrderEventConsumer.Presentation/Ozon.Route256.Kafka.OrderEventConsumer.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Ozon.Route256.Kafka.OrderEventConsumer.Presentation"
RUN dotnet build "Ozon.Route256.Kafka.OrderEventConsumer.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Ozon.Route256.Kafka.OrderEventConsumer.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ozon.Route256.Kafka.OrderEventConsumer.Presentation.dll"]
