using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin()
    .WithVolume("pgdata", "/var/lib/postgresql/data");

var db = postgres.AddDatabase("newsdb");

var dbMigrator = builder.AddProject<News_DbMigrator>("db-migrator")
    .WithReference(db)
    .WaitFor(postgres);

var newsParser = builder.AddProject<NewsParser>("news-parser")
    .WithReference(db)
    .WaitFor(dbMigrator);

var ollama = builder.AddContainer("ollama", "ollama/ollama")
    .WithEndpoint(11434, 11434, name: "http")
    .WithVolume("ollama-data", "/root/.ollama")
    .WithArgs("serve")
    .WithContainerRuntimeArgs("--gpus", "all");

builder.AddContainer("ollama-init", "curlimages/curl")
    .WithArgs("sh", "-c",
        "sleep 10 && curl http://ollama:11434/api/pull -d '{\"name\":\"qwen3.5:2b\"}'")
    .WaitFor(ollama);

builder.AddProject<Projects.News_AISummarizator>("news-aisummarizator")
    .WithReference(db)
    .WithReference(ollama.GetEndpoint("http"))
    .WaitFor(dbMigrator)
    .WaitFor(ollama);

builder.Build().Run();
