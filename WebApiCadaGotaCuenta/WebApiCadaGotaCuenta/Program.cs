using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using WebApiCadaGotaCuenta.Filters;
using WebApiCadaGotaCuenta.Middlewares;
using WebApiCadaGotaCuenta.Models;
using WebApiCadaGotaCuenta.Services;

var builder = WebApplication.CreateBuilder(args);
string connectionString;
string secret;

bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
if (!isDevelopment)
{
    connectionString = builder.Configuration["CONNECTION_STRING"];
    secret = builder.Configuration["ClaveJWT"];
}
else
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    secret = builder.Configuration["ClaveJWT"];
}

builder.Services.AddControllers(options =>
{
    options.Filters.Add<FiltroExcepcion>();
}).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddDbContext<GlucoControlContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);

builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddTransient<IGestorArchivos, GestorArchivosLocal>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = true,
                   ValidateIssuerSigningKey = true,
                   IssuerSigningKey = new SymmetricSecurityKey(
                     Encoding.UTF8.GetBytes(secret))
               });
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

//app.UseMiddleware<LogGetMiddleware>();

app.UseMiddleware<LogFileBodyHttpResponseMiddleware>();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
