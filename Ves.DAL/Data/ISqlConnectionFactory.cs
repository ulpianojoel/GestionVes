namespace Ves.DAL.Data;

public interface ISqlConnectionFactory
{
    string Name { get; }

    string ConnectionString { get; }
}
