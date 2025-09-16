# GestionVes
Suite de gestión con arquitectura en capas y una interfaz WPF moderna para escritorio.

## Arquitectura del repositorio
- **GestionVes.sln**: solución de Visual Studio con todos los proyectos.
- **Ves.Domain**: contratos y objetos de configuración compartidos.
- **Ves.DAL**: fábricas de conexión a SQL Server.
- **Ves.BLL**: registro centralizado de fábricas a partir de la configuración.
- **Ves.Services**: servicios reutilizables (diagnóstico de inicio, utilidades para la UI).
- **Ves.UI**: aplicación WPF (.NET 8, `WinExe`) con login, panel principal, gestión de usuarios, operaciones, reportes y estado de conexiones.

### Compatibilidad del código
El código fuente se reescribió usando únicamente características clásicas de C# (clases, propiedades con `get/set`, namespaces con llaves, etc.).
De esta manera la solución puede compilarse incluso en instalaciones de Visual Studio que todavía no habilitan las
anotaciones de nulabilidad o los records introducidos en versiones recientes del lenguaje. Aun así, seguí siendo
necesario contar con el SDK de .NET 8 para generar los binarios.

## Configuración requerida
La UI carga las cadenas de conexión desde `Ves.UI/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "Business": "<cadena de conexión para la base de datos principal>",
    "Hash": "<cadena de conexión para la base de datos de hash>"
  }
}
```

El archivo debe residir junto al ejecutable (`bin/<config>/<tfm>/`). La aplicación lo lee mediante `System.Text.Json`, por lo que no requiere paquetes NuGet adicionales, valida la presencia de ambas claves al iniciar y muestra un mensaje descriptivo si falta alguna.

## Credenciales de demostración
La capa de UI incluye un servicio simulado (`FakeAuthService`) con usuarios preconfigurados:

| Usuario    | Contraseña      | Rol           | Estado    |
|------------|-----------------|---------------|-----------|
| `admin`    | `Admin@2024`    | Administrador | Activo    |
| `analista` | `Analista@2024` | Analista      | Activo    |
| `auditoria`| `Auditoria@2024`| Auditor       | Inactivo  |

Ingresá con alguno de los usuarios activos para explorar las pantallas.

## Pantallas incluidas
- **Login y recuperación**: formulario moderno, mensajes de error y ventana para recuperar la contraseña.
- **Panel principal**: indicadores clave, flujo de operaciones y notificaciones recientes.
- **Gestión de usuarios**: listado con filtros, detalle del usuario seleccionado y acción para enviar recuperación.
- **Operaciones**: listado editable (estado, vencimiento y marcado como completado).
- **Reportes**: accesos rápidos a exportaciones y snapshots de indicadores.
- **Conexiones**: reporte de diagnóstico, visualización y copia de las cadenas cargadas.

La interfaz utiliza diseños con `Grid` y `Border` para adaptarse al tamaño de la ventana y mantener una estética consistente.

## Restaurar, compilar y ejecutar
1. **Restaurar dependencias** (soluciona `project.assets.json` faltante). No se utilizan paquetes externos, pero el comando garantiza que el SDK genere los archivos de soporte:
   ```bash
   dotnet restore GestionVes.sln
   ```
2. **Compilar la solución**:
   ```bash
   dotnet build GestionVes.sln -c Debug
   ```
3. **Ejecutar la UI WPF**:
   ```bash
   dotnet run --project Ves.UI/Ves.UI.csproj
   ```
   En Windows también podés iniciar desde Visual Studio estableciendo `Ves.UI` como proyecto de inicio y presionando **F5**.

### Configuración en Visual Studio
- Asegurate de que `Ves.UI` tenga:
  - `Output type`: **Windows Application (WinExe)**
  - `Target framework`: **.NET 8.0**
- Si el ejecutable no aparece tras compilar:
  1. **Build → Clean Solution** y luego **Build → Rebuild Solution**.
  2. Eliminá las carpetas `bin/` y `obj/` de cada proyecto y recompilá.
  3. Verificá que el SDK de .NET 8 esté instalado (ver sección siguiente).

## Errores comunes y solución
- **`DateTime` u otros tipos básicos no encontrados**: instalá o repará el SDK/targeting pack de .NET 8 y volvé a ejecutar `dotnet restore` / `dotnet build`.
- **`project.assets.json` ausente**: ejecutar `dotnet restore GestionVes.sln` o la opción **Restore NuGet Packages** en Visual Studio para regenerar los archivos internos del SDK (no se necesitan paquetes externos).
- **Faltan archivos de referencia (Microsoft.Win32...xml)**: indica una instalación de .NET dañada; reinstalá el SDK de .NET 8 y reiniciá Visual Studio.

## Bases de datos de ejemplo
En la carpeta `sql/` (crear manualmente si deseás) podés guardar los scripts provistos anteriormente para `BusinessDb` y `HashDb`. Ajustá las cadenas en `appsettings.json` para apuntar a tu servidor SQL Server.
