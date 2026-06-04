# MediTrack - Medical Appointment Service Context

## Stack

- ASP.NET Core 8 Web API
- MySql.EntityFrameworkCore 8.0.8
- Clean Architecture / DDD

## Structure

MedicalAppointmentManagement/
- Domain/Model/Aggregates/
- Domain/Model/Commands/
- Domain/Model/Queries/
- Domain/Model/ValueObjects/
- Application/Internal/CommandServices/
- Application/Internal/QueryServices/
- Infrastructure/Persistence/EFC/Configuration/
- Interfaces/REST/Controllers/
- Interfaces/REST/Resources/
- Interfaces/REST/Transform/

## Implemented requirements

- APT-RF1: schedule a medical appointment.
- APT-RF2: validate that appointment dates are in the future.
- APT-RF3: expose appointment preparation requirements.
- APT-RF4: edit or cancel future appointments.
- APT-RF5: block modification of past appointments.
- APT-RF6: register and query clinical exams pending pickup.

## Database

- Connection name: `AppoinmentDB`
- Hostname: `127.0.0.1`
- Port: `3306`
- Username: `root`
- Database: `AppoinmentDB`
