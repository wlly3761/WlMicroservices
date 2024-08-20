FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Ocelot.APIGateway/Ocelot.APIGateway.csproj", "Ocelot.APIGateway/"]
RUN dotnet restore "Ocelot.APIGateway/Ocelot.APIGateway.csproj"
COPY . .
WORKDIR "/src/Ocelot.APIGateway"
RUN dotnet build "Ocelot.APIGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ocelot.APIGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ocelot.APIGateway.dll"]
