var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("redis");
var postgres = builder.AddPostgres("postgres")
                      .WithDataVolume();

var db = postgres.AddDatabase("epicalcdi-db");

builder.AddProject<Projects.EpicalCDI_Api>("api")
       .WithReference(redis)
       .WithReference(db);

builder.AddNpmApp("web", "../EpicalCDI.Web")
       .WithHttpEndpoint(env: "PORT")
       .WithExternalHttpEndpoints();

builder.Build().Run();
