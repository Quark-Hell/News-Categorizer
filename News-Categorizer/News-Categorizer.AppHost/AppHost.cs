using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres").WithPgAdmin();
var db = postgres.AddDatabase("newsdb");

var dbMigrator = builder.AddProject<News_DbMigrator>("db-migrator")
    .WithReference(db)
    .WaitFor(postgres);

var newsParser = builder.AddProject<NewsParser>("news-parser")
    .WithReference(db)
    .WaitFor(dbMigrator);

builder.Build().Run();
