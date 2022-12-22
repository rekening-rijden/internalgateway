using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var host = builder.Host;

// Add services to the container.

services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
services.AddOcelot().AddKubernetes();
services.AddHealthChecks();
services.AddMvc(options => options.EnableEndpointRouting = false);

host.ConfigureAppConfiguration(config => config.AddJsonFile("ocelot.json"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseMvc();

app.MapHealthChecks("/health");

app.UseOcelot().Wait();

app.Run();
