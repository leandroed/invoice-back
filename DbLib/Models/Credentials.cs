using Newtonsoft.Json;

namespace DbLib.Models;

/// <summary>
/// User and database Credentials.
/// </summary>
public class Credentials
{
    /// <summary>
    /// Gets or sets database type.
    /// </summary>
    /// <value>Database type.</value>
    public string Engine { get; set; }

    /// <summary>
    /// Gets or sets username.
    /// </summary>
    /// <value>Username.</value>
    public string Username { get; set; }

    /// <summary>
    /// Gets or sets Password.
    /// </summary>
    /// <value>Password.</value>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets server address.
    /// </summary>
    /// <value>Server address.</value>
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets database name sid or schema.
    /// </summary>
    /// <value>Database sid or schema.</value>
    public string Dbname { get; set; }

    /// <summary>
    /// Gets or sets database port.
    /// </summary>
    /// <value>Database port.</value>
    public string Port { get; set; }
}
