FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["PruebaTecnica.API/PruebaTecnica.API.csproj", "PruebaTecnica.API/"]
COPY ["PruebaTecnica.Application/PruebaTecnica.Application.csproj", "PruebaTecnica.Application/"]
COPY ["PruebaTecnica.Domain/PruebaTecnica.Domain.csproj", "PruebaTecnica.Domain/"]
COPY ["PruebaTecnica.Infrastructure/PruebaTecnica.Infrastructure.csproj", "PruebaTecnica.Infrastructure/"]
COPY ["PruebaTecnica.Shared/PruebaTecnica.Shared.csproj", "PruebaTecnica.Shared/"]
RUN dotnet restore "PruebaTecnica.API/PruebaTecnica.API.csproj"
COPY . .
WORKDIR "/src/PruebaTecnica.API"
RUN dotnet build "PruebaTecnica.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PruebaTecnica.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PruebaTecnica.API.dll"] 