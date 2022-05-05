FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["GBank.Api/GBank.Api.csproj", "GBank.Api/"]
COPY ["GBank.Infrastructure/GBank.Infrastructure.csproj", "GBank.Infrastructure/"]
COPY ["GBank.Domain/GBank.Domain.csproj", "GBank.Domain/"]
RUN dotnet restore "GBank.Api/GBank.Api.csproj"
COPY . .
WORKDIR "/src/GBank.Api"
RUN dotnet build "GBank.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GBank.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GBank.Api.dll"]