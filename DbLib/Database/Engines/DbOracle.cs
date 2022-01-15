using System;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;
using Serilog;

namespace DbLib.Database;

/// <summary>
/// Database oracle connection.
/// </summary>
public class DbOracle : IDbType
{
    /// <summary>
    /// Oracle prefix parameter.
    /// </summary>
    private string prefixParam = ":";

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
    /// Start oracle connection.
    /// </summary>
    /// <param name="connectionString">Database string configuration.</param>
    /// <returns>Database connection.</returns>
    public DbConnection StartConnection(string connectionString)
    {
        DbConnection dbConn;
        try
        {
            dbConn = new OracleConnection(connectionString);
        }
        catch (Exception e)
        {
            Log.Error("It wasn't possible connect to Oracle db. " + e.StackTrace);
            dbConn = null;
        }

        return dbConn;
    }
}
