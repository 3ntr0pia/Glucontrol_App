using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;
using DiabetesNoteBook.Infrastructure.Interfaces;
using DiabetesNoteBook.Domain.Models;
using DiabetesNoteBook.Application.Interfaces;
using DiabetesNoteBook.Application.Services;
using DiabetesNoteBook.Application.Filters;
using Microsoft.EntityFrameworkCore.Design.Internal;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using DiabetesNoteBook.Infrastructure.Repositories.AddOperations;
using DiabetesNoteBook.Infrastructure.Repositories.UpdateOperations;
using DiabetesNoteBook.Infrastructure.Repositories.DeleteOperations;
using DiabetesNoteBook.Infrastructure.Repositories.OtherOperations;
using DiabetesNoteBook.Infrastructure.Repositories.GetOperations;
using DiabetesNoteBook.Application.Classes;
using DiabetesNoteBook.Infrastructure.Repositories;
using DiabetesNoteBook.Application.Services.Genereics;

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
//Servicio Email
builder.Services.AddScoped<IEmailService, EmailService>();//no eliminar
builder.Services.AddTransient<IConfirmEmailService, ConfirmEmailService>();//no eliminar
builder.Services.AddTransient<INewStringGuid, NewStringGuid>();//no eliminar
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();//para las vista
//Servicio de generacion de hash y token
builder.Services.AddTransient<HashService>();//no eliminar
builder.Services.AddTransient<TokenService>();//no eliminar
//Servicio de registro
builder.Services.AddScoped<INewRegister, NewRegisterService>();//no eliminar
//Servicio de gestion de contraseña
builder.Services.AddTransient<IChangePassService, ChangePassService>();//no eliminar
builder.Services.AddTransient<IChangePassMail, ChangePassEnlace>();//no eliminar
//Servicio de gestion datos del usuario
builder.Services.AddTransient<IBajaUsuarioServicio, BajaUsuarioService>();//no eliminar
builder.Services.AddTransient<IDeleteUserService, DeleteUserService>();//no eliminar
builder.Services.AddTransient<IChangeUserDataService, ChangeUserDataService>();//no eliminar
builder.Services.AddTransient<IUsuarioRepositoryEmailAndUsername, UsuarioRepositoryEmailAndUsername>();//no eliminar
builder.Services.AddTransient<IActualizacionYEnvioDeCorreoElectronico, EnvioYActualizacionDeCorreoRepository>();//no eliminar
builder.Services.AddTransient<IDeleteUserServices, DeleteUserServices>();//no eliminar
//Servicio de gestion mediciones
builder.Services.AddTransient<INuevaMedicionService, NuevaMedicionService>();//no eliminar
builder.Services.AddTransient<IDeleteMedicionService, DeleteMedicionService>();//no eliminar
builder.Services.AddTransient<IMedicionesIdUsuario, GetMedicionesIdUsuario>();//no eliminar
builder.Services.AddTransient<IDeleteMedicionServices, DeleteMedicionServices>();//no eliminar

//Servicio de gestion de medicacion
builder.Services.AddTransient<IAddNuevaMedicacion, AddNuevaMedicacionRepository>();//no eliminar
builder.Services.AddTransient<INewMedicationService, NewMedicationService>();//no eliminar
builder.Services.AddTransient<IConsultMedication, ConsultMedication>();//no eliminar
builder.Services.AddTransient<IDeleteMedication, DeleteMedicationService>();//no eliminar
builder.Services.AddTransient<IDeleteUserMedication, DeleteUserMedication>();
builder.Services.AddTransient<INewMedicacion, NewMedicationRepository>();//no eliminar
builder.Services.AddTransient<INewUsuarioMedicacion, SaveUsuarioMedicacionRepository>();//no eliminar
builder.Services.AddTransient<ExistMedicionesService>();
builder.Services.AddTransient<ExistUsersService>();

//----------------------------








builder.Services.AddMvc();
builder.Services.AddAuthentication();
builder.Logging.AddLog4Net();

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
