FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
WORKDIR /app

#Copy csproj and restore depenencies in an independent layer.
COPY ["src/Pokedex/Pokedex.Api.csproj", "src/Pokedex.Api/"]
COPY ["src/Pokedex.Contract/Pokedex.Contract.csproj", "src/Pokedex.Contract/"]
RUN dotnet restore "src/Pokedex.Api/Pokedex.Api.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "src\Pokedex\Pokedex.Api.csproj" -c Release -o out


#Generate runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build-env ./app/out .
EXPOSE 80
ENTRYPOINT ["dotnet", "Pokedex.Api.dll"]
