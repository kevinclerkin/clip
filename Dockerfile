# .NET runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use Python version (3.9) with .NET runtime image
FROM python:3.9-buster AS python
RUN apt-get update \
    && apt-get install -y \
        wget \
        gnupg \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Install Python packages
RUN pip install --no-cache-dir rembg pillow imageio    

# Find the path of the libpython3.8.so file and save it to a file
RUN find / -name "libpython3.9.so" > /libpython_path.txt

# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ClipAPI/ClipAPI.csproj", "ClipAPI/"]
RUN dotnet restore "ClipAPI/ClipAPI.csproj"
COPY . .
WORKDIR "/src/ClipAPI"
RUN dotnet build "ClipAPI.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "ClipAPI.csproj" -c Release -o /app/publish

# Use the .NET runtime image as the final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy Python files to the final image
COPY --from=python /usr/local /usr/local
COPY --from=python /usr/lib /usr/lib
COPY --from=python /libpython_path.txt /libpython_path.txt

# Print environment variables for debugging purposes
RUN cat /etc/environment

ENTRYPOINT ["dotnet", "ClipAPI.dll"]