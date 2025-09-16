using System.IO;
using System.Text.Json;
using Ves.BLL.Services;
using Ves.DAL.Data;
using Ves.Domain.Configuration;
using Ves.Services.Diagnostics;

namespace Ves.UI;

internal static class Program
{
    private const string AppSettingsFileName = "appsettings.json";

    private static int Main(string[] args)
    {
        if (!TryLoadConnectionOptions(out var options))
        {
            return 1;
        }

        var registry = new ConnectionFactoryRegistry(options);
        var diagnostics = new StartupDiagnosticsService(registry);

        if (!TryEnsureFactory(diagnostics, "Business", out var businessFactory))
        {
            return 1;
        }

        if (!TryEnsureFactory(diagnostics, "Hash", out var hashFactory))
        {
            return 1;
        }

        Console.WriteLine(diagnostics.BuildReport());
        PrintFactorySummary(businessFactory, hashFactory);
        WaitForExit();
        return 0;
    }

    private static bool TryLoadConnectionOptions(out DatabaseConnectionOptions options)
    {
        string baseDirectory = AppContext.BaseDirectory;
        string configPath = Path.Combine(baseDirectory, AppSettingsFileName);

        if (!File.Exists(configPath))
        {
            Console.Error.WriteLine($"No se encontró '{AppSettingsFileName}' en '{baseDirectory}'.");
            Console.Error.WriteLine("Copiá el archivo de configuración junto al ejecutable o actualizá la ruta de salida.");
            options = null!;
            return false;
        }

        try
        {
            using FileStream stream = File.OpenRead(configPath);
            using JsonDocument document = JsonDocument.Parse(stream);

            if (!document.RootElement.TryGetProperty("ConnectionStrings", out JsonElement connectionStrings))
            {
                Console.Error.WriteLine("El archivo de configuración debe incluir la sección ConnectionStrings.");
                options = null!;
                return false;
            }

            if (!TryReadConnectionString(connectionStrings, "Business", out string? business, out string? readError))
            {
                if (!string.IsNullOrEmpty(readError))
                {
                    Console.Error.WriteLine(readError);
                }
                options = null!;
                return false;
            }

            if (!TryReadConnectionString(connectionStrings, "Hash", out string? hash, out readError))
            {
                if (!string.IsNullOrEmpty(readError))
                {
                    Console.Error.WriteLine(readError);
                }
                options = null!;
                return false;
            }

            if (!DatabaseConnectionOptions.TryCreate(business, hash, out options, out string? validationError))
            {
                Console.Error.WriteLine(validationError);
                options = null!;
                return false;
            }

            return true;
        }
        catch (JsonException ex)
        {
            Console.Error.WriteLine($"El archivo '{AppSettingsFileName}' no tiene un formato JSON válido: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.Error.WriteLine($"No se pudo leer '{AppSettingsFileName}': {ex.Message}");
        }

        options = null!;
        return false;
    }

    private static bool TryReadConnectionString(JsonElement connectionStrings, string name, out string? value, out string? errorMessage)
    {
        if (!connectionStrings.TryGetProperty(name, out JsonElement element) || element.ValueKind == JsonValueKind.Null)
        {
            value = null;
            errorMessage = null;
            return true;
        }

        if (element.ValueKind == JsonValueKind.String)
        {
            value = element.GetString();
            errorMessage = null;
            return true;
        }

        value = null;
        errorMessage = $"ConnectionStrings:{name} debe ser una cadena.";
        return false;
    }

    private static bool TryEnsureFactory(StartupDiagnosticsService diagnostics, string name, out ISqlConnectionFactory factory)
    {
        if (diagnostics.TryGetFactory(name, out var resolved) && resolved is not null)
        {
            factory = resolved;
            return true;
        }

        Console.Error.WriteLine($"No se pudo inicializar la fábrica de conexiones '{name}'.");
        factory = null!;
        return false;
    }

    private static void PrintFactorySummary(ISqlConnectionFactory businessFactory, ISqlConnectionFactory hashFactory)
    {
        Console.WriteLine("Instancias disponibles: {0}, {1}.", businessFactory.Name, hashFactory.Name);
    }

    private static void WaitForExit()
    {
        Console.WriteLine("Presione una tecla para salir...");
        Console.ReadKey(intercept: true);
    }
}
