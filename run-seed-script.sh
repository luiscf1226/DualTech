#!/bin/bash

# Script to run the seed-data.sql script against the SQL Server container

# Set variables
CONTAINER_NAME="pruebatecnica-db-1"
SA_PASSWORD="YourStrong!Passw0rd"
DATABASE_NAME="PruebaTecnicaDb"
SCRIPT_PATH="./seed-data.sql"

echo "Running seed data script against SQL Server container..."

# Check if the container is running
if ! docker ps | grep -q $CONTAINER_NAME; then
    echo "Error: Container $CONTAINER_NAME is not running."
    echo "Please start the container with 'docker-compose up -d' first."
    exit 1
fi

# Check if the script file exists
if [ ! -f "$SCRIPT_PATH" ]; then
    echo "Error: Script file $SCRIPT_PATH not found."
    exit 1
fi

# Use a temporary container with the SQL Server tools to execute the script
echo "Executing SQL script using a temporary container with SQL tools..."
docker run --rm -v "$(pwd)/$SCRIPT_PATH:/tmp/seed-data.sql" --network container:$CONTAINER_NAME mcr.microsoft.com/mssql-tools /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "$SA_PASSWORD" -d "$DATABASE_NAME" -i /tmp/seed-data.sql

# Check if the script execution was successful
if [ $? -eq 0 ]; then
    echo "Seed data script executed successfully."
else
    echo "Error: Failed to execute seed data script."
    exit 1
fi

echo "Done." 