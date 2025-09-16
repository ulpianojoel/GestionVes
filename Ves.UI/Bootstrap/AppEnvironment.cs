using Microsoft.Extensions.Configuration;
using Ves.BLL.Services;
using Ves.Domain.Configuration;
using Ves.Services.Diagnostics;

namespace Ves.UI.Bootstrap;

public sealed class AppEnvironment
{
    public AppEnvironment(
        IConfigurationRoot configuration,
        DatabaseConnectionOptions options,
        ConnectionFactoryRegistry registry,
        StartupDiagnosticsService diagnostics)
    {
        Configuration = configuration;
        Options = options;
        Registry = registry;
        Diagnostics = diagnostics;
    }

    public IConfigurationRoot Configuration { get; }

    public DatabaseConnectionOptions Options { get; }

    public ConnectionFactoryRegistry Registry { get; }

    public StartupDiagnosticsService Diagnostics { get; }
}
