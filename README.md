# GestionVes
Sistema de gestión estructurado en varias capas.

## Arquitectura del repositorio
- **GestionVes.sln**: solución de Visual Studio que referencia todos los proyectos.
- **Ves.Domain**: contiene contratos y objetos de configuración compartidos.
- **Ves.DAL**: expone fábricas de conexiones a SQL Server.
- **Ves.BLL**: registra las fábricas disponibles a partir de la configuración cargada.
- **Ves.Services**: ofrece servicios de diagnóstico y utilidades reutilizables por la UI.
- **Ves.UI**: aplicación de consola que inicializa la configuración y muestra el estado de las conexiones.

## Configuración requerida
El proyecto espera un archivo `Ves.UI/appsettings.json` con el siguiente formato:

```json
{
  "ConnectionStrings": {
    "Business": "<cadena de conexión para la base de datos principal>",
    "Hash": "<cadena de conexión para la base de datos de hash>"
  }
}
```

El archivo se busca en el directorio base de la aplicación, por lo que debe encontrarse junto al ejecutable. La compilación copia automáticamente `appsettings.json` a la carpeta de salida. Si falta el archivo, la sección `ConnectionStrings` o alguna de las claves `Business`/`Hash`, la aplicación mostrará un mensaje descriptivo y finalizará con un código distinto de cero.

## Restaurar y compilar
1. **Restaurar dependencias** (soluciona el error `project.assets.json` no encontrado):
   ```bash
   dotnet restore GestionVes.sln
   ```
2. **Compilar toda la solución**:
   ```bash
   dotnet build GestionVes.sln -c Debug
   ```
3. **Ejecutar la UI**:
   ```bash
   dotnet run --project Ves.UI/Ves.UI.csproj
   ```

## Resolver "no se puede encontrar Ves.UI.exe" en Visual Studio
1. Abrí `GestionVes.sln` y marcá `Ves.UI` como **Startup Project**.
2. Verificá en `Ves.UI` → **Properties** → **Application** que `Output type` sea **Console Application** y `Target framework` sea **.NET 8.0**.
3. Ejecutá **Build → Clean Solution** y luego **Build → Rebuild Solution**. El panel **Output → Build** debe indicar `Build succeeded`.
4. Si persiste, cerrá Visual Studio, eliminá las carpetas `bin/` y `obj/` de cada proyecto (`Ves.Domain`, `Ves.DAL`, `Ves.BLL`, `Ves.Services`, `Ves.UI`) y volvé a abrir la solución.

## Diagnóstico adicional
- El mensaje `No se encontró el archivo de recursos ... project.assets.json` desaparece tras ejecutar `dotnet restore GestionVes.sln` o **Restore NuGet Packages** en Visual Studio.
- Para revisar el estado del arranque sin ejecutar la aplicación, consultá el reporte que imprime la consola. Muestra qué fábricas de conexión fueron registradas a partir del `appsettings.json`.
