# -----------------------------
# Build stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy project file first (for layer caching)
COPY Backend.csproj .
RUN dotnet restore

# Copy the rest of the source code
COPY . .
RUN dotnet publish -c Release -o /app/publish

# -----------------------------
# Runtime stage
# -----------------------------
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# The backend listens on port 80 inside the container
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "Backend.dll"]        