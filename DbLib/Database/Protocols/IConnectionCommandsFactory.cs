namespace DbLib.Database;

/// <summary>
/// Connection command factory.
/// </summary>
public interface IConnectionCommandsFactory
{
    /// <summary>
    /// Creates a connection command instance.
    /// </summary>
    /// <returns>Connection command.</returns>
    IConnectionCommands Create();
}
