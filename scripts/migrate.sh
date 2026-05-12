#!/bin/bash
# Run EF Core migrations

set -e

PROJECT="src/BMedia.Infrastructure"
STARTUP_PROJECT="src/BMedia.API"

echo "Creating initial migration..."
dotnet ef migrations add InitialCreate \
  --project $PROJECT \
  --startup-project $STARTUP_PROJECT \
  --output-dir Persistence/Migrations

echo "Applying migrations to database..."
dotnet ef database update \
  --project $PROJECT \
  --startup-project $STARTUP_PROJECT

echo "Done."
