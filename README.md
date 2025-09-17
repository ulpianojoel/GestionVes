# GestionVes

Sistema de gestión VES construido con .NET 8.

## Estructura de carpetas

- **Ves.Domain**: Entidades de dominio, value objects y enumeraciones.
- **Ves.DAL**: Acceso a datos utilizando ADO.NET y patrón Repository.
- **Ves.Services**: Servicios transversales como notificaciones, auditoría y unit of work.
- **Ves.BLL**: Lógica de negocio y orquestación de casos de uso.
- **Ves.UI**: Aplicación de consola que ejerce como interfaz de usuario.

La solución se orienta a SQL Server 2019 con dos bases de datos: una para datos de negocio y otra para hashes de archivos.

## Scripts de base de datos

En la carpeta `DatabaseScripts` se incluyen los scripts para crear ambas bases de datos (`VesDB` y `VesHashDB`).

Cada script crea la base de datos si no existe, genera el inicio de sesión SQL `test` (con contraseña `test` y las políticas deshabilitadas para aceptar la contraseña simple) y lo agrega como usuario `test` con permisos de `db_owner` en la base correspondiente.
