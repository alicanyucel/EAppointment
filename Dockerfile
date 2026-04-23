# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["EAppointment.WebApi/EAppointment.WebApi.csproj", "EAppointment.WebApi/"]
COPY ["EAppointment.Application/EAppointment.Application.csproj", "EAppointment.Application/"]
COPY ["EAppointment.Domain/EAppointment.Domain.csproj", "EAppointment.Domain/"]
COPY ["EAppointment.Infrastructure/EAppointment.Infrastructure.csproj", "EAppointment.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "EAppointment.WebApi/EAppointment.WebApi.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/EAppointment.WebApi"
RUN dotnet build "EAppointment.WebApi.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "EAppointment.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EAppointment.WebApi.dll"]
