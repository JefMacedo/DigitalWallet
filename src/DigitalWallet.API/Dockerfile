FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY src/DigitalWallet.API/DigitalWallet.API.csproj ./DigitalWallet.API/
COPY src/DigitalWallet.Application/DigitalWallet.Application.csproj ./DigitalWallet.Application/
COPY src/DigitalWallet.Domain/DigitalWallet.Domain.csproj ./DigitalWallet.Domain/
COPY src/DigitalWallet.Infrastructure/DigitalWallet.Infrastructure.csproj ./DigitalWallet.Infrastructure/
COPY src/DigitalWallet.Persistence/DigitalWallet.Persistence.csproj ./DigitalWallet.Persistence/

RUN dotnet restore ./DigitalWallet.API/DigitalWallet.API.csproj

COPY src/ .

WORKDIR /src/DigitalWallet.API
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "DigitalWallet.API.dll"]
