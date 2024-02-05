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
//Logica para hostearlo en ASPNETCORE
bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
//Si no esta en desarrollo coje estos datos
if (!isDevelopment)
{
    connectionString = builder.Configuration["CONNECTION_STRING"];
    secret = builder.Configuration["ClaveJWT"];
}
else
{
    //Si esta en desarrollo coje estos datos
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    secret = builder.Configuration["ClaveJWT"];
}

builder.Services.AddControllers(options =>
{
    //Agregado FiltroExcepcion
    options.Filters.Add<FiltroExcepcion>();
    //Esta line que hay a continuacion evita  que ocurra un error cuando se consulta en varias tablas
}).AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
//Agrega el contexto de la base de datos
//Inyeccion de dependencias
builder.Services.AddDbContext<DiabetesNoteBookContext>(options =>
{
    //Agrega la cadena de conexion que se encuentra en el archivo de sercretos
    options.UseSqlServer(connectionString);
    //Desactiva el tranking
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
}
);
//Estos servicios se ponen aqui para que puedan ser usados por otros controllers
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
//Agrega desde donde esta permitido acceder
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
//Necesario para que los servicios funcionen
builder.Services.AddHttpContextAccessor();
//Necesario para acceder a los endpoint
builder.Services.AddEndpointsApiExplorer();
//Toda la logica de autentificacion
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
//Si se usan cors hay que ponerlo
app.UseCors();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
