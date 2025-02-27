using Microsoft.EntityFrameworkCore;
using SaleProject.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SaleProjectContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("connectionDb"));
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("http//localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
var app = builder.Build();
app.UseCors("AllowAngular");

app.MapGet("/", () => "Hello World!");

app.MapControllers();
app.Run();
