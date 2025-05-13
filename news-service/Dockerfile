# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY ["news-service/news-service.API/news-service.API.csproj", "news-service.API/"]

RUN dotnet restore "news-service.API/news-service.API.csproj"

# Копируем все остальные файлы и собираем проект
COPY . .

# Публикация
RUN dotnet publish "news-service/news-service.API/news-service.API.csproj" -c Release -o /app/publish

# Создаем образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Копируем собранное приложение
COPY --from=build /app/publish .

# Указываем команду для старта приложения
ENTRYPOINT ["dotnet", "news-service.API.dll"]
