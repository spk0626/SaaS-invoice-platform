FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/Invoice.API/Invoice.API.csproj",                       "Invoice.API/"]
COPY ["src/Invoice.Application/Invoice.Application.csproj",       "Invoice.Application/"]
COPY ["src/Invoice.Domain/Invoice.Domain.csproj",                 "Invoice.Domain/"]
COPY ["src/Invoice.Infrastructure/Invoice.Infrastructure.csproj", "Invoice.Infrastructure/"]

RUN dotnet restore "Invoice.API/Invoice.API.csproj"

COPY src/ .
WORKDIR "/src/Invoice.API"

RUN dotnet build  -c Release -o /app/build
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser \
    && chown -R appuser /app
USER appuser

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

EXPOSE 8080
ENTRYPOINT ["dotnet", "Invoice.API.dll"]