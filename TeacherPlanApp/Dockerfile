# Стейдж 1: Сборка (Build)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ["TeacherPlanApp.csproj", "./"]
RUN dotnet restore "./TeacherPlanApp.csproj"

# Копируем весь остальной код и собираем проект
COPY . .
RUN dotnet publish "TeacherPlanApp.csproj" -c Release -o /app/publish

# Стейдж 2: Запуск (Final)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Устанавливаем curl (он нужен для healthcheck внутри контейнера)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Копируем только готовые скомпилированные файлы из первого стейджа
COPY --from=build /app/publish .

# Сообщаем докеру, что приложение будет работать на порту 8080
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

ENTRYPOINT ["dotnet", "TeacherPlanApp.dll"]