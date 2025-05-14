FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

COPY ["news-service/news-service.API/news-service.API.csproj", "news-service.API/"]

RUN dotnet restore "news-service.API/news-service.API.csproj"

COPY . .

RUN dotnet publish "news-service/news-service.API/news-service.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "news-service.API.dll"]