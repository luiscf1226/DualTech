#!/bin/bash

# Wait for SQL Server to be ready
sleep 30s

# Create the database if it doesn't exist
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -Q "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'PruebaTecnicaDb') BEGIN CREATE DATABASE PruebaTecnicaDb; END"

# Run the initialization script
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong!Passw0rd -i /docker-entrypoint-initdb.d/init-db.sql

echo "Database initialization completed." 