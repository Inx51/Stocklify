using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var faker = builder.AddProject<Stocklify_Faker>("faker")
    .WithEnvironment("Application__Capacity", "10")
    .WithEnvironment("Application__NumberOfStocksInMarket", "100")
    .WithEnvironment("Application__MinChangeRate", "10")
    .WithEnvironment("Application__MaxChangeRate", "20")
    .WithOtlpExporter();

builder.AddProject<Stocklify_Faker_ProtocolGateway>("faker-protocol-gateway")
    .WithReference(faker)
    .WithEnvironment("Application__StockValueServiceClientEndpoint", faker.Resource.GetEndpoint("https"))
    .WaitFor(faker);

builder.Build().Run();