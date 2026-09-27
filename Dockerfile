FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build-env
WORKDIR /app

COPY ./*sln ./

COPY ./src/TileDataExtract/*.csproj ./src/TileDataExtract/

COPY ./test/TileDataExtract.Tests/*.csproj ./test/TileDataExtract.Tests/

RUN dotnet restore --packages ./packages

COPY . ./
WORKDIR /app/src/TileDataExtract
RUN dotnet publish -c Release -o out --packages ./packages

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:10.0-alpine
WORKDIR /app

RUN apk add --no-cache curl

COPY --from=build-env /app/src/TileDataExtract/out .
ENTRYPOINT ["dotnet", "TileDataExtract.dll"]
