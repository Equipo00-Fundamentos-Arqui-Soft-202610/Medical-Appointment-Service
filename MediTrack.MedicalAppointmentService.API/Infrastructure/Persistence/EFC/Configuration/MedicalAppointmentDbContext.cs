using MediTrack.MedicalAppointmentService.API.Domain.Model.Aggregates;
using MediTrack.MedicalAppointmentService.API.Domain.Model.ValueObjects;
using MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC;
using Microsoft.EntityFrameworkCore;

namespace MediTrack.MedicalAppointmentService.API.Infrastructure.Persistence.EFC.Configuration;

public class MedicalAppointmentDbContext : DbContext
{
    public MedicalAppointmentDbContext(DbContextOptions<MedicalAppointmentDbContext> options)
        : base(options) { }

    public DbSet<MedicalAppointment> MedicalAppointments { get; set; } = null!;
    public DbSet<AppointmentRequirement> AppointmentRequirements { get; set; } = null!;
    public DbSet<ClinicalExam> ClinicalExams { get; set; } = null!;
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MedicalAppointment>(entity =>
        {
            entity.ToTable("appointments");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.PatientId)
                .HasColumnName("patient_id")
                .IsRequired();

            entity.Property(e => e.Type)
                .HasColumnName("type")
                .HasMaxLength(50)
                .IsRequired()
                .HasConversion(v => v.Value, v => AppointmentType.From(v));

            entity.Property(e => e.ScheduledAt)
                .HasColumnName("appointment_date")
                .IsRequired();

            entity.Property(e => e.Location)
                .HasColumnName("location")
                .HasMaxLength(255);
            
            entity.Property(e => e.Notes)
                .HasColumnName("notes")
                .HasMaxLength(400);

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(50)
                .IsRequired()
                .HasConversion(v => v.Value, v => AppointmentStatus.From(v));

            entity.Property(e => e.CreatedAt)
                .HasColumnName("created_at")
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .HasColumnName("updated_at");

            entity.HasMany(e => e.Requirements)
                .WithOne(r => r.MedicalAppointment)
                .HasForeignKey(r => r.MedicalAppointmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<AppointmentRequirement>(entity =>
        {
            entity.ToTable("appointment_requirements");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.MedicalAppointmentId)
                .HasColumnName("appointment_id")
                .IsRequired();

            entity.Property(e => e.Description)
                .HasColumnName("description")
                .HasMaxLength(500)
                .IsRequired()
                .HasConversion(v => v.Value, v => new RequirementText(v));
        });

        modelBuilder.Entity<ClinicalExam>(entity =>
        {
            entity.ToTable("clinical_exams");
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Id)
                .HasColumnName("id")
                .ValueGeneratedOnAdd();

            entity.Property(e => e.PatientId)
                .HasColumnName("patient_id")
                .IsRequired();

            entity.Property(e => e.AppointmentId)
                .HasColumnName("appointment_id");

            entity.Property(e => e.ExamType)
                .HasColumnName("exam_type")
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.ScheduledDate)
                .HasColumnName("scheduled_date");

            entity.Property(e => e.PickupDate)
                .HasColumnName("pickup_date");

            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasMaxLength(20)
                .IsRequired()
                .HasConversion(v => v.Value, v => ClinicalExamStatus.From(v));

            entity.HasOne(e => e.Appointment)
                .WithMany()
                .HasForeignKey(e => e.AppointmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.ToTable("outbox_message");
            entity.HasKey(m => m.Id);

            entity.Property(m => m.EventType).HasMaxLength(100).IsRequired();
            entity.Property(m => m.Payload).HasColumnType("json").IsRequired();
            entity.Property(m => m.OccurredAtUtc).IsRequired();
            entity.Property(m => m.LastError).HasMaxLength(500);

            entity.HasIndex(m => m.ProcessedAtUtc);
        });
    }
}
