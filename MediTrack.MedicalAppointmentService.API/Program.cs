using System.Text;
using MediTrack.MedicalAppointmentService.API.Application.Internal.CommandServices;
using MediTrack.MedicalAppointmentService.API.Application.Internal.QueryServices;
using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Messaging;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC.Configuration;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Security;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT authentication -- valores reales vía user-secrets en desarrollo, vía
// variables de entorno en producción. Nunca en appsettings.json (ver README).
builder.Services.AddOptions<JwtOptions>()
    .Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
    .Validate(o => !string.IsNullOrWhiteSpace(o.Key), "Jwt:Key es obligatorio")
    .Validate(o => !string.IsNullOrWhiteSpace(o.Issuer), "Jwt:Issuer es obligatorio")
    .Validate(o => !string.IsNullOrWhiteSpace(o.Audience), "Jwt:Audience es obligatorio")
    .ValidateOnStart();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>()
    ?? throw new InvalidOperationException("Falta la sección 'Jwt' en la configuración.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Key)),
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

// Patrón Outbox: los eventos se persisten en la misma BD que el cambio de
// dominio y se entregan a RabbitMQ en background (no se pierden si el broker
// está caído justo al publicar).
builder.Services.AddScoped<IEventPublisher, OutboxEventPublisher>();
builder.Services.AddHostedService<OutboxDispatcherHostedService>();

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
