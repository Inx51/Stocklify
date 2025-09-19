using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// var faker = builder.AddProject<Stocklify_Faker>("faker")
//     .WithEnvironment("Application__Capacity", "10")
//     .WithEnvironment("Application__NumberOfStocksInMarket", "20")
//     .WithEnvironment("Application__MinChangeRate", "10")
//     .WithEnvironment("Application__MaxChangeRate", "20")
//     .WithOtlpExporter();
//
// var fakerProtocolGateway = builder.AddProject<Stocklify_Faker_ProtocolGateway>("faker-protocol-gateway")
//     .WithReference(faker)
//     .WithEnvironment("Application__StockValueServiceClientEndpoint", faker.Resource.GetEndpoint("https"))
//     .WaitFor(faker);

// builder.AddNpmApp("frontend-react", "../Stocklify.Frontend/stocklify.react")
//     .WithReference(fakerProtocolGateway)
//     .WaitFor(fakerProtocolGateway)
//     .WithHttpEndpoint(env: "PORT", targetPort:3000)
//     .WithEnvironment("RUNS_ASPIRE", "true")
//     .WithExternalHttpEndpoints()
//     .PublishAsDockerFile();

builder.Build().Run();