#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["WebApiBankingSystem/WebApiBankingSystem.csproj", "WebApiBankingSystem/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Persistance/Persistance.csproj", "Persistance/"]
RUN dotnet restore "WebApiBankingSystem/WebApiBankingSystem.csproj"
COPY . .
WORKDIR "/src/WebApiBankingSystem"
RUN dotnet build "WebApiBankingSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebApiBankingSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WebApiBankingSystem.dll"]