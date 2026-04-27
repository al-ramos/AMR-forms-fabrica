FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/RDS.Forms.Fabrica.Domain/RDS.Forms.Fabrica.Domain.csproj src/RDS.Forms.Fabrica.Domain/
COPY src/RDS.Forms.Fabrica.Application/RDS.Forms.Fabrica.Application.csproj src/RDS.Forms.Fabrica.Application/
COPY src/RDS.Forms.Fabrica.Infrastructure/RDS.Forms.Fabrica.Infrastructure.csproj src/RDS.Forms.Fabrica.Infrastructure/
COPY src/RDS.Forms.Fabrica.API/RDS.Forms.Fabrica.API.csproj src/RDS.Forms.Fabrica.API/

RUN dotnet restore src/RDS.Forms.Fabrica.API/RDS.Forms.Fabrica.API.csproj

COPY src/ src/
RUN dotnet publish src/RDS.Forms.Fabrica.API/RDS.Forms.Fabrica.API.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
COPY src/RDS.Forms.Fabrica.API/rds_fabrica.db .
COPY seed.sql .

RUN sqlite3 rds_fabrica.db < seed.sql

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "RDS.Forms.Fabrica.API.dll"]
