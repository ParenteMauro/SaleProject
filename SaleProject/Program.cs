using Microsoft.EntityFrameworkCore;
using SaleProject.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SaleProjectContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("connectionDb"));
});
builder.Services.AddControllers();
var app = builder.Build();


app.MapGet("/", () => "Hello World!");

app.MapControllers();
app.Run();
