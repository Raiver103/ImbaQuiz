# Базовый образ для ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл csproj и восстанавливаем зависимости
COPY log-service.API/log-service.API.csproj log-service.API/
RUN dotnet restore log-service.API/log-service.API.csproj

# Копируем все остальные файлы
COPY . .

# Сборка проекта
WORKDIR /src/log-service.API
RUN dotnet build log-service.API.csproj -c Release -o /app/build

FROM build AS publish
# Публикация проекта
RUN dotnet publish log-service.API.csproj -c Release -o /app/publish

# Стартовый образ
FROM base AS final
WORKDIR /app

# Копируем из образа публикации
COPY --from=publish /app/publish .

# Настроим точку входа
ENTRYPOINT ["dotnet", "log-service.API.dll"]
