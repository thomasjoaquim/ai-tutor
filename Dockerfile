# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["MyProject.csproj", "."]
RUN dotnet restore "./MyProject.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MyProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyProject.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyProject.dll"]