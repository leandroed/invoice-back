using System;
using System.Data.Common;
using System.Data.SqlClient;
using Serilog;

namespace DbLib.Database;

/// <summary>
/// Database sql server connection.
/// </summary>
public class DbSqlServer : IDbType
{
    /// <summary>
    /// Sql server prefix parameter.
    /// </summary>
    private string prefixParam = "@";

    /// <summary>
    /// Gets or sets the database prefix parameter.
    /// </summary>
    /// <value>Database prefix parameter.</value>
    string IDbType.PrefixParam {
        get
        {
            return this.prefixParam;
        }
        set
        {
            this.prefixParam = value;
        }
    }

    /// <summary>
    /// Start sql server connection.
    /// </summary>
    /// <param name="connectionString">Database string configuration.</param>
    /// <returns>Database connection.</returns>
    public DbConnection StartConnection(string connectionString)
    {
        DbConnection connection;
        try
        {
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        catch (Exception e)
        {
            Log.Error("It wasn't possible connect to SqlServer db. " + e.StackTrace);
            connection = null;
        }

        return connection;
    }
}
