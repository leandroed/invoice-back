namespace DbLib.Database;

/// <summary>
/// Connection commands factory.
/// </summary>
public class ConnectionCommandsFactory : IConnectionCommandsFactory
{
    /// <summary>
    /// Creates a connection commands instance.
    /// </summary>
    /// <returns>Connection commands instance.</returns>
    public IConnectionCommands Create()
    {
        return new ConnectionCommands(Connection.Conn, Connection.Transaction);
    }
}
