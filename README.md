# Medical-Appointment-Service

Microservicio backend de MediTrack para gestionar citas medicas y examenes clinicos.

## Secretos en desarrollo local

`Jwt:Key` está vacío en `appsettings.json` a propósito -- es compartido con el
Gateway, Identity Service, Treatment-service, FollowUp-Service y Reminder-Service.
Cada dev lo configura una vez en su máquina:

```bash
dotnet user-secrets set "Jwt:Key" "<pedile la clave al equipo>" --project MediTrack.MedicalAppointmentService.API
```

En producción esa misma variable se setea como `Jwt__Key` en el entorno del
proveedor de deploy (Render, etc.) -- nunca en un archivo del repo.

## Stack

- ASP.NET Core 8 Web API
- Entity Framework Core 8
- MySql.EntityFrameworkCore 8.0.8
- Swagger / OpenAPI
- Clean Architecture + DDD en un unico proyecto API

## Endpoints principales

- `POST /api/v1/appointments`
- `GET /api/v1/appointments?patientId={id}`
- `GET /api/v1/appointments/{id}`
- `PUT /api/v1/appointments/{id}`
- `PATCH /api/v1/appointments/{id}/cancel`
- `POST /api/v1/appointments/{id}/attendance`
- `POST /api/v1/clinical-exams`
- `GET /api/v1/clinical-exams/pending?patientId={id}`
- `PATCH /api/v1/clinical-exams/{id}/picked-up`
