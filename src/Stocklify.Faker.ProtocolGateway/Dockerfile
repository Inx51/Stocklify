FROM mcr.microsoft.com/dotnet/sdk:9.0 as build
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:9.0
ENV ASPNETCORE_URLS=http://+:8080
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet", "Stocklify.Faker.ProtocolGateway.dll"]
EXPOSE 8080