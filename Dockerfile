FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./CatVsDogsBinaryClassification.sln ./ ./
COPY ./ModelBuilder/ModelBuilder.csproj ./app/ModelBuilder/ModelBuilder.csproj
COPY ./CatsVsDogs.Api/CatsVsDogs.Api.csproj ./app/CatsVsDogs.Api/CatsVsDogs.Api.csproj
COPY ./CatsVsDogs.Core/CatsVsDogs.Core.csproj ./app/CatsVsDogs.Core/CatsVsDogs.Core.csproj
COPY ./CatsVsDogs.Infrastructure/CatsVsDogs.Infrastructure.csproj ./app/CatsVsDogs.Infrastructure/CatsVsDogs.Infrastructure.csproj

RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

EXPOSE 5432

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY CatsVsDogs.Api/MLModel/model.zip .
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CatsVsDogs.Api.dll"]
