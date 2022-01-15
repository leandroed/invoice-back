using DbLib.Models;

namespace DbLib.Database;

/// <summary>
/// Sql server connection string.
/// </summary>
public class SqlServerConnectionString : DatabaseConnectionString
{
    /// <summary>
    /// Create a database connection string.
    /// </summary>
    /// <param name="dbModel">Database model.</param>
    /// <returns>String connection.</returns>
    public override string CreateStringConnection(Credentials dbModel)
    {
        return this.ValidateProperties(dbModel) ? "Data Source=" + dbModel.Host + ";" +
            "Initial Catalog=" + dbModel.Dbname + ";" +
            "Integrated Security=false;" +
            "User ID=" + dbModel.Username + ";" +
            "Password=" + dbModel.Password + ";" +
            "MultipleActiveResultSets=True" + ";" : string.Empty;
    }

    /// <summary>
    /// Validate database properties.
    /// </summary>
    /// <param name="dbModel">Database model.</param>
    /// <returns>True: Properties valid | False: invalid property.</returns>
    public override bool ValidateProperties(Credentials dbModel)
    {
        return dbModel != null && IsValidDatabaseModel(dbModel);
    }
}
