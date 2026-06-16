FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GlobalLogisticsManagementSystem/GlobalLogisticsManagementSystem.csproj", "GlobalLogisticsManagementSystem/"]
RUN dotnet restore "GlobalLogisticsManagementSystem/GlobalLogisticsManagementSystem.csproj"
COPY . .
WORKDIR "/src/GlobalLogisticsManagementSystem"
RUN dotnet build "GlobalLogisticsManagementSystem.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GlobalLogisticsManagementSystem.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GlobalLogisticsManagementSystem.dll"]