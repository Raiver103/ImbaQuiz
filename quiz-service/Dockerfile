# Используем официальный образ для ASP.NET Core
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# Используем официальный образ для сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ImbaQuiz.API/ImbaQuiz.API.csproj", "ImbaQuiz.API/"]
COPY ["ImbaQuiz.Application/ImbaQuiz.Application.csproj", "ImbaQuiz.Application/"]
COPY ["ImbaQuiz.Domain/ImbaQuiz.Domain.csproj", "ImbaQuiz.Domain/"]
COPY ["ImbaQuiz.Infrastructure/ImbaQuiz.Infrastructure.csproj", "ImbaQuiz.Infrastructure/"]
RUN dotnet restore "ImbaQuiz.API/ImbaQuiz.API.csproj"
COPY . .
WORKDIR "/src/ImbaQuiz.API"
RUN dotnet build "ImbaQuiz.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ImbaQuiz.API.csproj" -c Release -o /app/publish

# Стартовый образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ImbaQuiz.API.dll"]
