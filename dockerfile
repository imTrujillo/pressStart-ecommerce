# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ./Shop.Presentation/ ./Shop.Presentation/
COPY ./Shop.Application/ ./Shop.Application/
COPY ./Shop.Domain/ ./Shop.Domain/
COPY ./Shop.Infrastructure/ ./Shop.Infrastructure/

RUN dotnet publish Shop.Presentation/Shop.Presentation.csproj -c Release -o /app

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app .

EXPOSE 5000
ENTRYPOINT ["dotnet", "Shop.Presentation.dll"]
