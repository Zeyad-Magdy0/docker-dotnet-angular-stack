# Dockerized .NET + Angular Stack

This repository demonstrates a Dockerized full-stack application using modern container-based architecture.

## Tech Stack
- Backend: ASP.NET Core (.NET)
- Frontend: Angular (served as static files via NGINX)
- Database: Microsoft SQL Server
- Cache: Redis
- Orchestration: Docker Compose

## Architecture Overview
Client requests are handled by NGINX, which serves Angular static files and proxies API requests to the ASP.NET Core backend.  
The backend communicates with Redis for caching and SQL Server for persistent storage.

## Project Structure
docker-dotnet-angular-stack/
├── docker-compose.yml
├── backend/
│   ├── Dockerfile
│   ├── Program.cs
│   ├── appsettings.json
│   ├── Backend.csproj
│   └── backend.sln
├── frontend/
│   ├── Dockerfile
│   ├── nginx.conf
│   └── dist/
│       └── index.html
├── db/
│   └── init.sql
├── redis/
│   └── redis.conf
├── Output/
│   ├── Health.png
│   ├── Users.png
│   ├── Data.png
│   ├── Cache-Hit.png
│   └── Cache-Miss.png
├── System Design.drawio
├── System Design Visualization.png
├── Execution Flow.txt
└── File System Design.txt

## Running the Project
Requirements:
- Docker
- Docker Compose

Start the application:
docker compose up --build

## Access Points
- Frontend: http://localhost:8080
- Health Check: http://localhost:8080/api/health
- Users API: http://localhost:8080/api/users
- Cached Data API: http://localhost:8080/api/data

## API Endpoints
- GET /api/health  
  Returns backend health status

- GET /api/users  
  Fetches users from SQL Server

- GET /api/data  
  Demonstrates Redis cache-aside pattern

## Database Initialization
The database is initialized using db/init.sql:
- Creates database if it does not exist
- Creates Users table
- Inserts sample data

## Redis Cache
Redis is used as an in-memory cache to demonstrate:
- Cache-aside pattern
- Reduced database load
- TTL-based cache expiration

## Networking Design
- frontend-net: Frontend ↔ Backend
- backend-net (internal): Backend ↔ Redis ↔ SQL Server

This improves security and service isolation.

## Purpose
This project demonstrates:
- Docker and Docker Compose proficiency
- Multi-container architecture
- Backend and frontend separation
- Caching strategies
- Clean filesystem organization
- Clear system documentation

## Author
Zeyad Magdy  
Cloud / DevOps / Backend Engineering

## Notes
This project is intended for educational and demonstration purposes.
