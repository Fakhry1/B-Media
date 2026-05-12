#!/bin/bash
# Quick dev environment setup

set -e

echo "Starting infrastructure services (PostgreSQL + Redis)..."
docker-compose up -d postgres redis

echo "Waiting for PostgreSQL to be ready..."
until docker-compose exec postgres pg_isready -U bmedia_user -d bmedia_db; do
  sleep 2
done

echo "Running EF Core migrations..."
cd "$(dirname "$0")/.."
dotnet ef migrations add InitialCreate \
  --project src/BMedia.Infrastructure \
  --startup-project src/BMedia.API \
  --output-dir Persistence/Migrations \
  2>/dev/null || echo "Migration already exists, skipping..."

dotnet ef database update \
  --project src/BMedia.Infrastructure \
  --startup-project src/BMedia.API

echo ""
echo "Setup complete! Run the API with:"
echo "  dotnet run --project src/BMedia.API"
echo ""
echo "Swagger UI: http://localhost:5000"
echo "Hangfire:   http://localhost:5000/hangfire"
echo "Health:     http://localhost:5000/health"
