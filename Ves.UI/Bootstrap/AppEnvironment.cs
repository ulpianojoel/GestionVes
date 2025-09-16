using Ves.BLL.Services;
using Ves.Domain.Configuration;
using Ves.Services.Diagnostics;

namespace Ves.UI.Bootstrap
{
    public sealed class AppEnvironment
    {
        public AppEnvironment(
            DatabaseConnectionOptions options,
            ConnectionFactoryRegistry registry,
            StartupDiagnosticsService diagnostics)
        {
            Options = options;
            Registry = registry;
            Diagnostics = diagnostics;
        }

        public DatabaseConnectionOptions Options { get; private set; }

        public ConnectionFactoryRegistry Registry { get; private set; }

        public StartupDiagnosticsService Diagnostics { get; private set; }
    }
}
