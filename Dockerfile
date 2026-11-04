FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TallerApi/TallerApi.csproj TallerApi/
RUN dotnet restore TallerApi/TallerApi.csproj

COPY TallerApi/ TallerApi/
RUN dotnet publish TallerApi/TallerApi.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080
ENTRYPOINT ASPNETCORE_URLS=http://+:${PORT:-8080} dotnet TallerApi.dll
