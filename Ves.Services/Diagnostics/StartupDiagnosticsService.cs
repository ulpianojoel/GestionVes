using System;
using System.Text;
using Ves.BLL.Services;
using Ves.DAL.Data;

namespace Ves.Services.Diagnostics;

public sealed class StartupDiagnosticsService
{
    private readonly ConnectionFactoryRegistry _registry;

    public StartupDiagnosticsService(ConnectionFactoryRegistry registry)
    {
        _registry = registry ?? throw new ArgumentNullException(nameof(registry));
    }

    public string BuildReport()
    {
        var builder = new StringBuilder();
        builder.AppendLine("VES UI arrancó correctamente.");
        builder.AppendLine("Se registraron las siguientes cadenas de conexión:");

        foreach (var name in _registry.RegisteredNames)
        {
            builder.Append(" - ");
            builder.AppendLine(name);
        }

        return builder.ToString();
    }

    public bool TryGetFactory(string name, out ISqlConnectionFactory? factory) =>
        _registry.TryGetFactory(name, out factory);
}
