# Medical-Appointment-Service
# Medical-Appointment-Service

Microservicio backend de MediTrack para gestionar citas medicas y examenes clinicos.

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
