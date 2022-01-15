using DbLib.Models;

namespace DbLib.Database;

/// <summary>
/// Database connection string.
/// </summary>
public abstract class DatabaseConnectionString
{
    /// <summary>
    /// Validate database fields has valid values.
    /// </summary>
    /// <param name="dbModel">Database model.</param>
    /// <returns>True: Model is valid | False: Invalid value found.</returns>
    public static bool IsValidDatabaseModel(Credentials dbModel)
    {
        if (dbModel == null)
        {
            return false;
        }

        List<string> properties = new List<string>();
        properties.Add(dbModel.Engine);
        properties.Add(dbModel.Username);
        properties.Add(dbModel.Password);
        properties.Add(dbModel.Host);
        properties.Add(dbModel.Dbname);

        return !properties.Contains(string.Empty) && !properties.Contains(null);
    }

    /// <summary>
    /// Create a database connection string.
    /// </summary>
    /// <param name="dbModel">Database model.</param>
    /// <returns>String connection.</returns>
    public abstract string CreateStringConnection(Credentials dbModel);

    /// <summary>
    /// Validate database properties.
    /// </summary>
    /// <param name="dbModel">Database model.</param>
    /// <returns>True: Properties valid | False: invalid property.</returns>
    public abstract bool ValidateProperties(Credentials dbModel);
}
