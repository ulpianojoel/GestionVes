using System;
using System.IO;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Ves.BLL.Services;
using Ves.Domain.Configuration;
using Ves.Services.Diagnostics;

namespace Ves.UI.Bootstrap
{
    public static class ConfigurationBootstrapper
    {
        private const string AppSettingsFileName = "appsettings.json";
        private const string BusinessConnectionName = "Business";
        private const string HashConnectionName = "Hash";

        public static bool TryInitialize(out AppEnvironment environment)
        {
            environment = null;

            IConfigurationRoot configuration;
            if (!TryBuildConfiguration(out configuration))
            {
                return false;
            }

            DatabaseConnectionOptions options;
            if (!TryCreateOptions(configuration, out options))
            {
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
                    .AddJsonFile(AppSettingsFileName, false, false)
                    .Build();
                return true;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show(
                    string.Format("No se encontró '{0}' junto al ejecutable en '{1}'.", AppSettingsFileName, AppContext.BaseDirectory),
                    "Configuración no encontrada",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(
                    string.Format("El archivo '{0}' tiene un formato inválido:\n{1}", AppSettingsFileName, ex.Message),
                    "Error de formato",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se pudo cargar la configuración:\n" + ex.Message,
                    "Error de configuración",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            configuration = null;
            return false;
        }

        private static bool TryCreateOptions(IConfiguration configuration, out DatabaseConnectionOptions options)
        {
            string business = configuration.GetConnectionString(BusinessConnectionName);
            string hash = configuration.GetConnectionString(HashConnectionName);

            string validationError;
            if (DatabaseConnectionOptions.TryCreate(business, hash, out options, out validationError))
            {
                return true;
            }

            MessageBox.Show(
                validationError ?? "No se pudieron validar las cadenas de conexión.",
                "Configuración incompleta",
                MessageBoxButton.OK,
                MessageBoxImage.Warning);

            options = null;
            return false;
        }
    }
}
