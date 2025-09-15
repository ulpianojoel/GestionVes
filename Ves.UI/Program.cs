using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.IO;

namespace Ves.UI;

internal static class Program
{
    private const string AppSettingsFileName = "appsettings.json";

    private static int Main(string[] args)
    {
        if (!TryLoadConfiguration(out IConfigurationRoot configuration))
        {
            return 1;
        }

        if (!TryCreateFactory(configuration, "Business", out SqlConnectionFactory businessFactory))
        {
            return 1;
        }

        if (!TryCreateFactory(configuration, "Hash", out SqlConnectionFactory hashFactory))
        {
            return 1;
        }

        PrintSuccessMessage(businessFactory, hashFactory);
        WaitForExit();
        return 0;
    }

    private static bool TryLoadConfiguration(out IConfigurationRoot configuration)
    {
        string baseDirectory = AppContext.BaseDirectory;
        string configPath = Path.Combine(baseDirectory, AppSettingsFileName);

        if (!File.Exists(configPath))
        {
            Console.Error.WriteLine($"No se encontró '{AppSettingsFileName}' en '{baseDirectory}'.");
            Console.Error.WriteLine("Copiá el archivo de configuración junto al ejecutable o actualizá la ruta de salida.");
            configuration = null!;
            return false;
        }

        configuration = new ConfigurationBuilder()
            .SetBasePath(baseDirectory)
            .AddJsonFile(AppSettingsFileName, optional: false, reloadOnChange: false)
            .Build();

        return true;
    }

    private static bool TryCreateFactory(IConfiguration configuration, string name, out SqlConnectionFactory factory)
    {
        string? connectionString = configuration.GetConnectionString(name);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            Console.Error.WriteLine($"La cadena de conexión '{name}' no está configurada en {AppSettingsFileName}.");
            Console.Error.WriteLine("Actualizá la sección ConnectionStrings del archivo para incluir un valor válido.");
            factory = null!;
            return false;
        }

        factory = new SqlConnectionFactory(connectionString);
        return true;
    }

    private static void PrintSuccessMessage(SqlConnectionFactory businessFactory, SqlConnectionFactory hashFactory)
    {
        Console.WriteLine("VES UI arrancó correctamente.");
        Console.WriteLine("Las cadenas de conexión se cargaron desde appsettings.json sin inconvenientes.");
        Console.WriteLine($"Instancias disponibles: {nameof(businessFactory)}, {nameof(hashFactory)}.");
    }

    private static void WaitForExit()
    {
        Console.WriteLine("Presione una tecla para salir...");
        Console.ReadKey(intercept: true);
    }
}

internal sealed class SqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection Create() => new SqlConnection(_connectionString);
}
