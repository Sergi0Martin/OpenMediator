using System.Reflection;
using OpenMediator;
using WebApplicationAPI.MediatorMiddleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddOpenMediator(config =>
{
    config.RegisterCommandsFromAssemblies([Assembly.GetExecutingAssembly()]);
    config.RegisterMiddleware<CustomMediatorMiddleware>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
