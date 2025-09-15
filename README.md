# GestionVes
Sistema de gestión

## Configuración

El proyecto espera un archivo `Ves.UI/appsettings.json` con el siguiente formato:

```json
{
  "ConnectionStrings": {
    "Business": "<cadena de conexión para la base de datos principal>",
    "Hash": "<cadena de conexión para la base de datos de hash>"
  }
}
```

Cada cadena de conexión se utiliza para crear instancias de `SqlConnectionFactory`. El archivo se busca en el directorio base de la aplicación, por lo que debe encontrarse junto al ejecutable. El proyecto copia automáticamente `appsettings.json` a la carpeta de salida en cada compilación, de modo que no es necesario moverlo manualmente.

## Resolver "no se puede encontrar Ves.UI.exe" en Visual Studio

Si Visual Studio informa que no puede iniciar `Ves.UI.exe`, seguí estos pasos para asegurarte de que la UI es un proyecto ejecutable y que la compilación genera el `.exe`:

1. **Elegí el proyecto correcto como inicio.** En el Solution Explorer, hacé clic derecho sobre `Ves.UI` y seleccioná **Set as Startup Project**.
2. **Confirmá el tipo de salida.** Abrí `Ves.UI` → **Properties** → **Application** y verificá que `Output type` sea **Console Application** y `Target framework` sea **.NET 8.0**.
3. **Comprobá el archivo de proyecto.** `Ves.UI/Ves.UI.csproj` contiene lo esencial para compilar como ejecutable:
   ```xml
   <Project Sdk="Microsoft.NET.Sdk">
     <PropertyGroup>
       <OutputType>Exe</OutputType>
       <TargetFramework>net8.0</TargetFramework>
       <ImplicitUsings>enable</ImplicitUsings>
       <Nullable>enable</Nullable>
     </PropertyGroup>
   </Project>
   ```
   Si ves valores diferentes, ajustalos desde Visual Studio o editando el archivo.
4. **Limpiá y reconstruí.** En el menú **Build**, elegí **Clean Solution** y luego **Rebuild Solution**. Revisá **View → Output (Build)** para confirmar que aparece `Build succeeded`.
5. **Restablecé la caché si persiste.** Cerrá Visual Studio, eliminá las carpetas `bin/` y `obj/` dentro de `Ves.UI`, volvé a abrir la solución y ejecutá un `Rebuild`.

## Compilar por línea de comandos

Para aislar problemas del IDE, podés construir y ejecutar la aplicación directamente con el SDK de .NET:

```bash
dotnet clean
dotnet build ./Ves.UI/Ves.UI.csproj -c Debug
dotnet run --project ./Ves.UI/Ves.UI.csproj
```

Si estos comandos funcionan, pero Visual Studio sigue sin poder iniciar la aplicación, el problema reside en la configuración del perfil de depuración del IDE.
