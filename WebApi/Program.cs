using Application.Contracts;
using Application.Services;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();
var configuration = builder.Configuration;

services.AddScoped<NpgsqlConnection>(_ =>
    new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));
services.AddTransient<IApiNewsService, CnnNewsService>();

services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();


app.Run();

