FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/AMR.Forms.Fabrica.Domain/AMR.Forms.Fabrica.Domain.csproj src/AMR.Forms.Fabrica.Domain/
COPY src/AMR.Forms.Fabrica.Application/AMR.Forms.Fabrica.Application.csproj src/AMR.Forms.Fabrica.Application/
COPY src/AMR.Forms.Fabrica.Infrastructure/AMR.Forms.Fabrica.Infrastructure.csproj src/AMR.Forms.Fabrica.Infrastructure/
COPY src/AMR.Forms.Fabrica.API/AMR.Forms.Fabrica.API.csproj src/AMR.Forms.Fabrica.API/

RUN dotnet restore src/AMR.Forms.Fabrica.API/AMR.Forms.Fabrica.API.csproj

COPY src/ src/
RUN dotnet publish src/AMR.Forms.Fabrica.API/AMR.Forms.Fabrica.API.csproj \
    -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN apt-get update && apt-get install -y sqlite3 && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .
COPY src/AMR.Forms.Fabrica.API/rds_fabrica.db .
COPY seed.sql .

RUN sqlite3 rds_fabrica.db < seed.sql

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "AMR.Forms.Fabrica.API.dll"]
