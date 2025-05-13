FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
 
COPY ["quiz-service/ImbaQuiz.API/ImbaQuiz.API.csproj", "ImbaQuiz.API/"]
COPY ["quiz-service/ImbaQuiz.Application/ImbaQuiz.Application.csproj", "ImbaQuiz.Application/"]
COPY ["quiz-service/ImbaQuiz.Domain/ImbaQuiz.Domain.csproj", "ImbaQuiz.Domain/"]
COPY ["quiz-service/ImbaQuiz.infrastructure/ImbaQuiz.infrastructure.csproj", "ImbaQuiz.infrastructure/"]

RUN dotnet restore "ImbaQuiz.API/ImbaQuiz.API.csproj"
 
COPY . .
RUN dotnet publish "quiz-service/ImbaQuiz.API/ImbaQuiz.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ImbaQuiz.API.dll"]
