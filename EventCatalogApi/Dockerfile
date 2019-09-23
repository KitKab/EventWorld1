FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app
EXPOSE 80

COPY . .
RUN dotnet restore EventCatalogApi.csproj

FROM build AS publish
RUN dotnet publish "EventCatalogApi.csproj" -c Release -o /app

WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "EventCatalogApi.dll"]