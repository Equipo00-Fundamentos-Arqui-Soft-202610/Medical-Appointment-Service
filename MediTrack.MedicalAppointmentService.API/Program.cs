using MediTrack.MedicalAppointmentService.API.Application.Internal.CommandServices;
using MediTrack.MedicalAppointmentService.API.Application.Internal.QueryServices;
using MediTrack.MedicalAppointmentService.API.Domain.Model;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC.Configuration;
using MediTrack.MedicalAppointmentService.API.Interfaces.REST.Transform;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MedicalAppointmentDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!));

builder.Services.AddScoped<IMedicalAppointmentRepository, MedicalAppointmentRepository>();
builder.Services.AddScoped<IClinicalExamRepository, ClinicalExamRepository>();

builder.Services.AddScoped<IMedicalAppointmentCommandService, MedicalAppointmentCommandService>();
builder.Services.AddScoped<IMedicalAppointmentQueryService, MedicalAppointmentQueryService>();
builder.Services.AddScoped<IClinicalExamCommandService, ClinicalExamCommandService>();
builder.Services.AddScoped<IClinicalExamQueryService, ClinicalExamQueryService>();

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

app.UseAuthorization();
app.MapControllers();
app.Run();
