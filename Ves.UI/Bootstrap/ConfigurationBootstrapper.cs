using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Ves.BLL.Services;
using Ves.Domain.Configuration;
using Ves.Services.Diagnostics;

namespace Ves.UI.Bootstrap;

public static class ConfigurationBootstrapper
{
    private const string AppSettingsFileName = "appsettings.json";
    private const string BusinessConnectionName = "Business";
    private const string HashConnectionName = "Hash";

    public static bool TryInitialize(out AppEnvironment environment)
    {
        if (!TryBuildConfiguration(out var configuration))
        {
            environment = null!;
            return false;
        }

        if (!TryCreateOptions(configuration, out var options))
        {
            environment = null!;
            return false;
        }

        var registry = new ConnectionFactoryRegistry(options);
        var diagnostics = new StartupDiagnosticsService(registry);
        environment = new AppEnvironment(configuration, options, registry, diagnostics);
        return true;
    }

    private static bool TryBuildConfiguration(out IConfigurationRoot configuration)
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
            MessageBox.Show(
                $"No se encontró '{AppSettingsFileName}' junto al ejecutable en '{AppContext.BaseDirectory}'.",
                "Configuración no encontrada",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (FormatException ex)
        {
            MessageBox.Show(
                $"El archivo '{AppSettingsFileName}' tiene un formato inválido:\n{ex.Message}",
                "Error de formato",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"No se pudo cargar la configuración:\n{ex.Message}",
                "Error de configuración",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
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

        MessageBox.Show(
            validationError ?? "No se pudieron validar las cadenas de conexión.",
            "Configuración incompleta",
            MessageBoxButton.OK,
            MessageBoxImage.Warning);

        options = null!;
        return false;
    }
}
