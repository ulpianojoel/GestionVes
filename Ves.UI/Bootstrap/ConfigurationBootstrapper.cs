using System;
using System.IO;
using System.Text.Json;
using System.Windows;
using Ves.BLL.Services;
using Ves.Domain.Configuration;
using Ves.Services.Diagnostics;

namespace Ves.UI.Bootstrap
{
    public static class ConfigurationBootstrapper
    {
        private const string AppSettingsFileName = "appsettings.json";
        private const string ConnectionStringsSection = "ConnectionStrings";
        private const string BusinessConnectionName = "Business";
        private const string HashConnectionName = "Hash";

        public static bool TryInitialize(out AppEnvironment environment)
        {
            environment = null;

            DatabaseConnectionOptions options;
            if (!TryCreateOptions(out options))
            {
                return false;
            }

            var registry = new ConnectionFactoryRegistry(options);
            var diagnostics = new StartupDiagnosticsService(registry);
            environment = new AppEnvironment(options, registry, diagnostics);
            return true;
        }

        private static bool TryCreateOptions(out DatabaseConnectionOptions options)
        {
            string business;
            string hash;

            if (!TryReadConnectionStrings(out business, out hash))
            {
                options = null;
                return false;
            }

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

        private static bool TryReadConnectionStrings(out string business, out string hash)
        {
            business = null;
            hash = null;

            string path = Path.Combine(AppContext.BaseDirectory, AppSettingsFileName);

            if (!File.Exists(path))
            {
                MessageBox.Show(
                    string.Format("No se encontró '{0}' junto al ejecutable en '{1}'.", AppSettingsFileName, AppContext.BaseDirectory),
                    "Configuración no encontrada",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return false;
            }

            try
            {
                using (var stream = File.OpenRead(path))
                using (var document = JsonDocument.Parse(stream))
                {
                    JsonElement connectionStrings;
                    if (!document.RootElement.TryGetProperty(ConnectionStringsSection, out connectionStrings) || connectionStrings.ValueKind != JsonValueKind.Object)
                    {
                        MessageBox.Show(
                            "El archivo 'appsettings.json' no contiene la sección 'ConnectionStrings'.",
                            "Configuración incompleta",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);
                        return false;
                    }

                    string businessValue;
                    if (!TryGetRequiredString(connectionStrings, BusinessConnectionName, out businessValue))
                    {
                        return false;
                    }

                    string hashValue;
                    if (!TryGetRequiredString(connectionStrings, HashConnectionName, out hashValue))
                    {
                        return false;
                    }

                    business = businessValue;
                    hash = hashValue;
                    return true;
                }
            }
            catch (JsonException ex)
            {
                MessageBox.Show(
                    "El archivo 'appsettings.json' no tiene un formato JSON válido:\n" + ex.Message,
                    "Error de formato",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show(
                    "No se pudo leer 'appsettings.json':\n" + ex.Message,
                    "Error de lectura",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Ocurrió un error inesperado al cargar la configuración:\n" + ex.Message,
                    "Error de configuración",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }

            return false;
        }

        private static bool TryGetRequiredString(JsonElement parent, string propertyName, out string value)
        {
            JsonElement element;
            if (!parent.TryGetProperty(propertyName, out element) || element.ValueKind != JsonValueKind.String)
            {
                MessageBox.Show(
                    string.Format("Falta la clave '{0}' en la sección 'ConnectionStrings'.", propertyName),
                    "Configuración incompleta",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                value = null;
                return false;
            }

            value = element.GetString() ?? string.Empty;
            return true;
        }
    }
}
