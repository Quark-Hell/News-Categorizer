using Aspire.Hosting;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.News_Categorizer_ApiService>("apiservice")
    .WithHttpHealthCheck("/health");

builder.AddProject<Projects.News_Categorizer_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(apiService)
    .WaitFor(apiService);

var postgres = builder.AddPostgres("postgres");
var db = postgres.AddDatabase("newsdb");

var dbMigrator = builder.AddProject<News_DbMigrator>("db-migrator")
    .WithReference(db);

var newsParser = builder.AddProject<NewsParser>("news-parser")
    .WithReference(db)
    .WaitFor(dbMigrator);

builder.Build().Run();
