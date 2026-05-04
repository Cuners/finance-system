# См. статью по ссылке https://aka.ms/customizecontainer, чтобы узнать как настроить контейнер отладки и как Visual Studio использует этот Dockerfile для создания образов для ускорения отладки.

# Этот этап используется при запуске из VS в быстром режиме (по умолчанию для конфигурации отладки)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001


# Этот этап используется для сборки проекта службы
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FinanceService/BudgetServer/BudgetServer.csproj", "FinanceService/BudgetServer/"]
COPY ["FinanceService/Finance.Application/Finance.Application.csproj", "FinanceService/Finance.Application/"]
COPY ["FinanceService/Finance.Domain/Finance.Domain.csproj", "FinanceService/Finance.Domain/"]
COPY ["FinanceService/Finance.Infrastructure/Finance.Infrastructure.csproj", "FinanceService/Finance.Infrastructure/"]
COPY ["Finance.Contracts/Finance.Contracts.csproj", "Finance.Contracts/"]
RUN dotnet restore "./FinanceService/BudgetServer/BudgetServer.csproj"
COPY . .
WORKDIR "/src/FinanceService/BudgetServer"
RUN dotnet build "./BudgetServer.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Этот этап используется для публикации проекта службы, который будет скопирован на последний этап
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BudgetServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Этот этап используется в рабочей среде или при запуске из VS в обычном режиме (по умолчанию, когда конфигурация отладки не используется)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BudgetServer.dll"]