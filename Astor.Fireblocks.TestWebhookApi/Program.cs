using Astor.Fireblocks;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapPost("/webhook", (TransactionWebhook webhook) => webhook);

app.Run();
