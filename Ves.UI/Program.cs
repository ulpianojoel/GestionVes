using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Ves.BLL.Services;
using Ves.DAL.Data;
using Ves.Domain.Configuration;
using Ves.Services.Diagnostics;

namespace Ves.UI;

internal static class Program
{
    private const string AppSettingsFileName = "appsettings.json";
    private const string BusinessConnectionName = "Business";
    private const string HashConnectionName = "Hash";

    private static int Main(string[] args)
    {
        if (!TryBuildConfiguration(out var configuration))
        {
            return 1;
        }

        if (!TryCreateOptions(configuration, out var options))
        {
            return 1;
        }

        var registry = new ConnectionFactoryRegistry(options);
        var diagnostics = new StartupDiagnosticsService(registry);

        if (!TryEnsureFactory(diagnostics, BusinessConnectionName, out var businessFactory))
        {
            return 1;
        }

        if (!TryEnsureFactory(diagnostics, HashConnectionName, out var hashFactory))
        {
            return 1;
        }

        Console.WriteLine(diagnostics.BuildReport());
        PrintFactorySummary(businessFactory, hashFactory);
        WaitForExit();
        return 0;
    }

    private static bool TryBuildConfiguration(out IConfiguration configuration)
    {
        try
        {
            configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(AppSettingsFileName, optional: false, reloadOnChange: false)
                .Build();
            return true;
        }
        catch (FileNotFoundException)
        {
            Console.Error.WriteLine($"No se encontró '{AppSettingsFileName}' junto al ejecutable en '{AppContext.BaseDirectory}'.");
            Console.Error.WriteLine("Copiá el archivo de configuración o verificá la ruta de salida del proyecto.");
        }
        catch (FormatException ex)
        {
            Console.Error.WriteLine($"El archivo '{AppSettingsFileName}' tiene un formato inválido: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"No se pudo cargar la configuración: {ex.Message}");
        }

        configuration = null!;
        return false;
    }

    private static bool TryCreateOptions(IConfiguration configuration, out DatabaseConnectionOptions options)
    {
        string? business = configuration.GetConnectionString(BusinessConnectionName);
        string? hash = configuration.GetConnectionString(HashConnectionName);

        if (DatabaseConnectionOptions.TryCreate(business, hash, out options, out var validationError))
        {
            return true;
        }

        Console.Error.WriteLine(validationError);
        options = null!;
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
