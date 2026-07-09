using System.Text;
using MediTrack.MedicalAppointmentService.API.Application.Internal.CommandServices;
using MediTrack.MedicalAppointmentService.API.Application.Internal.QueryServices;
using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Messaging;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC.Configuration;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT authentication
var jwtSection = builder.Configuration.GetSection("Jwt");
var signingKey = jwtSection["Key"]
    ?? throw new InvalidOperationException("Falta la clave de firma JWT en 'Jwt:Key'.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSection["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSection["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey)),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<MedicalAppointmentDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IMedicalAppointmentRepository, MedicalAppointmentRepository>();
builder.Services.AddScoped<IClinicalExamRepository, ClinicalExamRepository>();

builder.Services.AddScoped<IMedicalAppointmentCommandService, MedicalAppointmentCommandService>();
builder.Services.AddScoped<IMedicalAppointmentQueryService, MedicalAppointmentQueryService>();
builder.Services.AddScoped<IClinicalExamCommandService, ClinicalExamCommandService>();
builder.Services.AddScoped<IClinicalExamQueryService, ClinicalExamQueryService>();

builder.Services.AddSingleton<IEventPublisher, RabbitMqPublisher>();

builder.Services.AddScoped<AppointmentCommandFromResourceAssembler>();
builder.Services.AddScoped<MedicalAppointmentResourceFromEntityAssembler>();
builder.Services.AddScoped<ClinicalExamCommandFromResourceAssembler>();
builder.Services.AddScoped<ClinicalExamResourceFromEntityAssembler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<MedicalAppointmentDbContext>();
    db.Database.Migrate();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
