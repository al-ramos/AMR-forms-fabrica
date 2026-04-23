FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY RDS.Forms.Fabrica.Domain/RDS.Forms.Fabrica.Domain.csproj RDS.Forms.Fabrica.Domain/
COPY RDS.Forms.Fabrica.Application/RDS.Forms.Fabrica.Application.csproj RDS.Forms.Fabrica.Application/
COPY RDS.Forms.Fabrica.Infrastructure/RDS.Forms.Fabrica.Infrastructure.csproj RDS.Forms.Fabrica.Infrastructure/
COPY RDS.Forms.Fabrica.Web/RDS.Forms.Fabrica.Web.csproj RDS.Forms.Fabrica.Web/

RUN dotnet restore RDS.Forms.Fabrica.Web/RDS.Forms.Fabrica.Web.csproj

COPY . .
RUN dotnet publish RDS.Forms.Fabrica.Web/RDS.Forms.Fabrica.Web.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "RDS.Forms.Fabrica.Web.dll"]