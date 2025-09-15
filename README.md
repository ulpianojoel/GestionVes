# GestionVes

Sistema de gestión VES construido con .NET 8.

## Estructura de carpetas

- **Ves.Domain**: Entidades de dominio, value objects y enumeraciones.
- **Ves.DAL**: Acceso a datos utilizando ADO.NET y patrón Repository.
- **Ves.Services**: Servicios transversales como notificaciones, auditoría y unit of work.
- **Ves.BLL**: Lógica de negocio y orquestación de casos de uso.
- **Ves.UI**: Aplicación de consola que ejerce como interfaz de usuario.

La solución se orienta a SQL Server 2019 en `DESKTOP-B7V8BS2\\SQLEXPRESS` con dos bases de datos: una para datos de negocio y otra para hashes de archivos.
