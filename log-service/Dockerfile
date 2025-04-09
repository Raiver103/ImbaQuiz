FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["log-service.API/log-service.API.csproj", "log-service.API/"]
RUN dotnet restore "log-service.API/log-service.API.csproj"
COPY . .
WORKDIR "/src/log-service.API"
RUN dotnet build "log-service.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "log-service.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "log-service.API.dll"]
