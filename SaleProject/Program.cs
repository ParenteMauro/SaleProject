using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SaleProject.Common;
using SaleProject.Models;
using SaleProject.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SaleProjectContext>(op =>
{
    op.UseSqlServer(builder.Configuration.GetConnectionString("connectionDb"));
});
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy =>
        {
            policy.WithOrigins("*")
                .AllowAnyMethod()
                .WithHeaders("*")
                .AllowAnyHeader();
                
        });
});
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);

//JWT
var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Secret);

builder.Services.AddAuthentication(d =>
{
    d.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    d.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
  .AddJwtBearer(d =>
  {
      d.RequireHttpsMetadata = false;
      d.SaveToken = true;
      d.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
      };

  });

builder.Services.AddControllers();
var app = builder.Build();
app.UseCors("AllowAngular");

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", () => "Hello World!");

app.MapControllers();
app.Run();
