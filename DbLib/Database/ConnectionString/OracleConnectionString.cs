using DbLib.Models;

namespace DbLib.Database;

/// <summary>
/// Oracle connection string.
/// </summary>
public class OracleConnectionString : DatabaseConnectionString
{
    /// <summary>
    /// Create oracle connection string.
    /// </summary>
    /// <param name="dbModel">Database properties.</param>
    /// <returns>Connection string.</returns>
    public override string CreateStringConnection(Credentials dbModel)
    {
        return this.ValidateProperties(dbModel) ? "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)" +
            "(HOST=" + dbModel.Host + ")(PORT=" + dbModel.Port + "))(CONNECT_DATA=(SID=" + dbModel.Dbname + ")));" +
            "User Id=" + dbModel.Username + ";Password=" + dbModel.Password + ";" : string.Empty;
    }

    /// <summary>
    /// Validate oracle database properties.
    /// </summary>
    /// <param name="dbModel">Database properties.</param>
    /// <returns>True: Valid properties | False: Invalid property.</returns>
    public override bool ValidateProperties(Credentials dbModel)
    {
        return dbModel == null && !string.IsNullOrEmpty(dbModel.Port) && IsValidDatabaseModel(dbModel);
    }
}
