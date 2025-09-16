using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ves.DAL.Data;
using Ves.Domain.Configuration;

namespace Ves.BLL.Services
{
    public sealed class ConnectionFactoryRegistry
    {
        private readonly ReadOnlyDictionary<string, ISqlConnectionFactory> _factories;

        public ConnectionFactoryRegistry(DatabaseConnectionOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var factories = new Dictionary<string, ISqlConnectionFactory>(StringComparer.OrdinalIgnoreCase)
            {
                { "Business", new SqlConnectionFactory("Business", options.Business) },
                { "Hash", new SqlConnectionFactory("Hash", options.Hash) }
            };

            _factories = new ReadOnlyDictionary<string, ISqlConnectionFactory>(factories);
        }

        public IReadOnlyCollection<string> RegisteredNames
        {
            get { return _factories.Keys; }
        }

        public bool TryGetFactory(string name, out ISqlConnectionFactory factory)
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
}
