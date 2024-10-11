using Application.Contracts;
using Application.Services;
using Infrastructure;
using Infrastructure.Contracts;
using Npgsql;
using WebApi.Contracts;
using WebApi.Middlewares;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services.AddEndpointsApiExplorer();
services.AddSwaggerGen();


builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

services.AddScoped<NpgsqlConnection>(_ =>
    new NpgsqlConnection(configuration.GetConnectionString("DefaultConnection")));

services.AddTransient<IApiNewsService, CnnNewsService>();
services.AddTransient<IAuthService, AuthService>();
services.AddTransient<IDbContext, DbContext>();
services.AddTransient<ITokenProvider, TokenProvider>();
services.AddTransient<IPasswordHasher, PasswordHasher>();

services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseMiddleware<HttpExceptionMiddleware>();
app.UseHttpsRedirection();
app.UseSession();
app.MapControllers();


app.Run();

