﻿FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/IM.IdentityService.Api/IM.IdentityService.Api.csproj", "IM.IdentityService.Api/"]
RUN dotnet restore "src/IM.IdentityService.Api/IM.IdentityService.Api.csproj"
COPY . .
WORKDIR "/src/IM.IdentityService.Api"
RUN dotnet build "IM.IdentityService.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IM.IdentityService.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IM.IdentityService.Api.dll"]
