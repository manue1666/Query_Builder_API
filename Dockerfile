# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["QueryBuilderAPI.csproj", "./"]
RUN dotnet restore "QueryBuilderAPI.csproj"

COPY . .
RUN dotnet build "QueryBuilderAPI.csproj" -c Release -o /app/build

# Publish stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS publish
WORKDIR /src
COPY --from=build /src .
RUN dotnet publish "QueryBuilderAPI.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=publish /app/publish .

# Ejecutar migraciones antes de iniciar
RUN dotnet ef database update || true

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "QueryBuilderAPI.dll"]