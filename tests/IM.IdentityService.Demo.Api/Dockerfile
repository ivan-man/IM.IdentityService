FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["tests/IM.IdentityService.Demo.Api/IM.IdentityService.Demo.Api.csproj", "IM.IdentityService.Demo.Api/"]
RUN dotnet restore "tests/IM.IdentityService.Demo.Api/IM.IdentityService.Demo.Api.csproj"
COPY . .
WORKDIR "/src/IM.IdentityService.Demo.Api"
RUN dotnet build "IM.IdentityService.Demo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IM.IdentityService.Demo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IM.IdentityService.Demo.Api.dll"]
