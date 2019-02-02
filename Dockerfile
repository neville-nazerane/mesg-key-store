FROM microsoft/dotnet:2.2-aspnetcore-runtime-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-stretch AS build
WORKDIR /src
COPY ["KeyStoreService/KeyStoreService.csproj", "KeyStoreService/"]
RUN dotnet restore "KeyStoreService/KeyStoreService.csproj"
COPY . .
WORKDIR "/src/KeyStoreService"
RUN dotnet build "KeyStoreService.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "KeyStoreService.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "KeyStoreService.dll"]