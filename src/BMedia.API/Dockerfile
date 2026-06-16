FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["src/BMedia.API/BMedia.API.csproj", "src/BMedia.API/"]
COPY ["src/BMedia.Application/BMedia.Application.csproj", "src/BMedia.Application/"]
COPY ["src/BMedia.Domain/BMedia.Domain.csproj", "src/BMedia.Domain/"]
COPY ["src/BMedia.Infrastructure/BMedia.Infrastructure.csproj", "src/BMedia.Infrastructure/"]

RUN dotnet restore "src/BMedia.API/BMedia.API.csproj"

COPY . .
WORKDIR "/src/src/BMedia.API"
RUN dotnet build "BMedia.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BMedia.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

RUN addgroup --system --gid 1001 appgroup && \
    adduser --system --uid 1001 --ingroup appgroup --no-create-home appuser && \
    mkdir -p /app/uploads /app/logs && \
    chown -R appuser:appgroup /app/uploads /app/logs && \
    chmod 755 /app/uploads /app/logs

COPY --from=publish /app/publish .
RUN chown -R appuser:appgroup /app

USER appuser

HEALTHCHECK --interval=30s --timeout=10s --start-period=60s --retries=3 \
  CMD curl -f http://localhost:8080/health || exit 1

ENV ASPNETCORE_URLS="http://+:8080;https://+:8081"
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "BMedia.API.dll"]
