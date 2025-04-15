# Используем официальный образ .NET SDK для сборки приложения
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY ["quiz-service/ImbaQuiz.API/ImbaQuiz.API.csproj", "ImbaQuiz.API/"]
COPY ["quiz-service/ImbaQuiz.Application/ImbaQuiz.Application.csproj", "ImbaQuiz.Application/"]
COPY ["quiz-service/ImbaQuiz.Domain/ImbaQuiz.Domain.csproj", "ImbaQuiz.Domain/"]
COPY ["quiz-service/ImbaQuiz.infrastructure/ImbaQuiz.infrastructure.csproj", "ImbaQuiz.infrastructure/"]

RUN dotnet restore "ImbaQuiz.API/ImbaQuiz.API.csproj"

# Копируем все остальные файлы и собираем проект
COPY . .
RUN dotnet publish "quiz-service/ImbaQuiz.API/ImbaQuiz.API.csproj" -c Release -o /app/publish

# Создаем образ для runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Копируем собранное приложение
COPY --from=build /app/publish .

# Указываем команду для старта приложения
ENTRYPOINT ["dotnet", "ImbaQuiz.API.dll"]
