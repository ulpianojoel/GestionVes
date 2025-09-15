using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace Ves.UI;

internal static class Program
{
    private const string AppSettingsFileName = "appsettings.json";

    private static void Main(string[] args)
    {
        IConfiguration configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile(AppSettingsFileName, optional: false, reloadOnChange: false)
            .Build();

        SqlConnectionFactory businessFactory = BuildFactory(configuration, "Business");
        SqlConnectionFactory hashFactory = BuildFactory(configuration, "Hash");

        Console.WriteLine("VES UI arrancó ok.");
        Console.WriteLine("Las cadenas de conexión se cargaron correctamente.");
        Console.WriteLine($"Instancias disponibles: {nameof(businessFactory)}, {nameof(hashFactory)}.");
        Console.WriteLine("Presione una tecla para salir...");
        Console.ReadKey(intercept: true);
    }

    private static SqlConnectionFactory BuildFactory(IConfiguration configuration, string name)
    {
        string? connectionString = configuration.GetConnectionString(name);

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException($"La cadena de conexión '{name}' no está configurada en {AppSettingsFileName}.");
        }

        return new SqlConnectionFactory(connectionString);
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
