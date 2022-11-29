Environment.SetEnvironmentVariable("BASEDIR", AppDomain.CurrentDomain.BaseDirectory);

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    ContentRootPath = AppDomain.CurrentDomain.BaseDirectory
});

var host = builder.Host;
var webHost = builder.WebHost;
var services = builder.Services;
var configuration = builder.Configuration;



// Add services to the container.

host.UseSerilog((context, config) => config.ReadFrom.Configuration(context.Configuration));
host.UseSystemd();

builder.Services.AddControllers()
    .AddNewtonsoftJson();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

host.ConfigureHostConfiguration(hostConf =>
{
    hostConf.AddJsonFile("hosting.json", true, true);
    hostConf.AddJsonFile("keys/authorized_key.json");
});

webHost.UseKestrel();


services.AddApplicationOptions(configuration);
services.AddApplicationServices(configuration);
services.ConfigureHttpClients(configuration);


var app = builder.Build();

var telegramBotWebHook  = app.Services.GetRequiredService<ITelegramWebHook>();
await telegramBotWebHook.DeleteAsync();
await telegramBotWebHook.SetAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();