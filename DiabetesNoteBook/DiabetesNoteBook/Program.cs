using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Infrastructure.Repositories;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Application.Filters;

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

builder.Services.AddDbContext<DiabetesNoteBookContext>(options =>
{
    options.UseSqlServer(connectionString);
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);

builder.Services.AddTransient<HashService>();
builder.Services.AddTransient<TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INewRegister, NewRegisterService>();
builder.Services.AddTransient<IConfirmEmailService, ConfirmEmailService>();
builder.Services.AddTransient<IChangePassService, ChangePassService>();
builder.Services.AddTransient<INewRegisterRepository, NewRegisterRepository>();
builder.Services.AddTransient<IConfirmationRegisterRepository, ConfirmationRegisterRepository>();
builder.Services.AddTransient<INewStringGuid, NewStringGuid>();
builder.Services.AddTransient<IOperationsService, OperationsService>();
builder.Services.AddTransient<IAddOperation, AddOperation>();
builder.Services.AddTransient<IChangePassword, ChangePassword>();
builder.Services.AddTransient<IChangePassMail, ChangePassEnlace>();
builder.Services.AddTransient<IChangePasswordMail, ChangePasswordMail>();
builder.Services.AddTransient<IUserDeregistration, UserDeregistration>();
builder.Services.AddTransient<IUserDeregistrationService, UserDeregistrationService>();
builder.Services.AddTransient<IDeleteUserService, DeleteUserService>();
builder.Services.AddTransient<IDeleteUser, DeleteUserRepository>();
builder.Services.AddTransient<IChangeUserDataService, ChangeUserDataService>();
builder.Services.AddTransient<IChangeUserData, ChangeUserData>();
builder.Services.AddTransient<ISaveNuevaMedicion,SaveNuevaMedicionRepository>();
builder.Services.AddTransient<INuevaMedicionService,NuevaMedicionService>();
builder.Services.AddTransient<IDeleteMedicion, DeleteMedicionRepository>();
builder.Services.AddTransient<IDeleteMedicionService, DeleteMedicionService>();

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

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
