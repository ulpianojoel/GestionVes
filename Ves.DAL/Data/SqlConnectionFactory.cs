namespace Ves.DAL.Data
{
    public sealed class SqlConnectionFactory : ISqlConnectionFactory
    {
        public SqlConnectionFactory(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }

        public string Name { get; private set; }

        public string ConnectionString { get; private set; }
    }
}
