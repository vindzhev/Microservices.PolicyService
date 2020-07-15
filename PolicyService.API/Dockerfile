#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["PolicyService/PolicyService.API/PolicyService.API.csproj", "PolicyService/PolicyService.API/"]
COPY ["PolicyService/PolicyService.Infrastructure/PolicyService.Infrastructure.csproj", "PolicyService/PolicyService.Infrastructure/"]
COPY ["PolicyService/PolicyService.Application/PolicyService.Application.csproj", "PolicyService/PolicyService.Application/"]
COPY ["PolicyService/PolicyService.Domain/PolicyService.Domain.csproj", "PolicyService/PolicyService.Domain/"]
RUN dotnet restore "PolicyService/PolicyService.API/PolicyService.API.csproj"
COPY . .
WORKDIR "/src/PolicyService/PolicyService.API"
RUN dotnet build "PolicyService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PolicyService.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PolicyService.API.dll"]
