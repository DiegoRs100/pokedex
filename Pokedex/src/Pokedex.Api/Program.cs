using Pokedex.Api;

var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder);

startup.ConfigureServices();

var app = builder.Build();
startup.ConfigureApplication(app);

app.Run();