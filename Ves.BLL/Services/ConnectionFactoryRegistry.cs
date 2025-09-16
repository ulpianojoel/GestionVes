using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Ves.DAL.Data;
using Ves.Domain.Configuration;

namespace Ves.BLL.Services;

public sealed class ConnectionFactoryRegistry
{
    private readonly ReadOnlyDictionary<string, ISqlConnectionFactory> _factories;

    public ConnectionFactoryRegistry(DatabaseConnectionOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var factories = new Dictionary<string, ISqlConnectionFactory>(StringComparer.OrdinalIgnoreCase)
        {
            ["Business"] = new SqlConnectionFactory("Business", options.Business),
            ["Hash"] = new SqlConnectionFactory("Hash", options.Hash)
        };

        _factories = new ReadOnlyDictionary<string, ISqlConnectionFactory>(factories);
    }

    public IReadOnlyCollection<string> RegisteredNames => _factories.Keys;

    public bool TryGetFactory(string name, [NotNullWhen(true)] out ISqlConnectionFactory? factory)
    {
        if (_factories.TryGetValue(name, out var resolved))
        {
            factory = resolved;
            return true;
        }

        factory = null;
        return false;
    }
}
