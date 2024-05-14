FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /WebApplication6

COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime
WORKDIR /WebApplication6
COPY --from=build /WebApplication6/out .
ENTRYPOINT ["dotnet", "WebApplication6.dll"]
