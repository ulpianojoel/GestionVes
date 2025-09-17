using Microsoft.Data.SqlClient;
using System;
using Ves.DAL.Interfaces;
using Ves.Services.Interfaces;

namespace Ves.Services.Implementations
{
    public class AdoNetUnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly IDbConnectionFactory _factory;
        private SqlConnection? _conn;
        private SqlTransaction? _tx;

        public AdoNetUnitOfWork(IDbConnectionFactory factory)
        {
            _factory = factory;
        }

        public void Begin()
        {
            _conn = _factory.CreateOpenConnection();   // <- antes: CreateBusinessConnection()
            _tx = _conn.BeginTransaction();
        }

        public void Commit() => _tx?.Commit();
        public void Rollback() => _tx?.Rollback();

        public SqlConnection Connection => _conn ??= _factory.CreateOpenConnection();
        public SqlTransaction? Transaction => _tx;

        public void Dispose()
        {
            _tx?.Dispose();
            _conn?.Dispose();
        }
    }
}
