using System.Data.Common;

namespace DbLib.Database;

/// <summary>
/// Database type interface.
/// </summary>
public interface IDbType
{
    /// <summary>
    /// Gets or sets the database prefix parameter.
    /// </summary>
    /// <value>Database prefix parameter.</value>
    string PrefixParam { get; set; }

    /// <summary>
    /// Start database connection.
    /// </summary>
    /// <param name="connectionString">String connection configuration.</param>
    /// <returns>Database connection.</returns>
    DbConnection StartConnection(string connectionString);
}
