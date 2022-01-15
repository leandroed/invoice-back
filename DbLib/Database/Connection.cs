using System.Data;
using System.Data.Common;
using DbLib.Enumerators;
using Serilog;

namespace DbLib.Database;

/// <summary>
/// Class connection.
/// </summary>
public static class Connection
{
    /// <summary>
    /// Database connection.
    /// </summary>
    [ThreadStatic]
    private static DbConnection conn;

    /// <summary>
    /// Connection type.
    /// </summary>
    private static IDbType connection;

    /// <summary>
    /// Gets or sets connection string.
    /// </summary>
    /// <value>Connection config.</value>
    public static string ConnectionString { get; set; }

    /// <summary>
    /// Gets or sets database type.
    /// </summary>
    /// <value>Database type.</value>
    public static EnumDatabase IdfDatabase { get; set; }

    /// <summary>
    /// Gets or sets a prefix used in parametrized querys.
    /// </summary>
    /// <value>Prefix for parameter.</value>
    public static string PrefixParam {
        get
        {
            return connection.PrefixParam;
        }
    }

    /// <summary>
    /// Gets database connection.
    /// </summary>
    /// <value>Connection.</value>
    public static DbConnection Conn
    {
        get
        {
            if (conn == null)
            {
                ConnectionFactory();
                conn = connection.StartConnection(ConnectionString);
            }

            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                Log.Error($"It wasn't possible start connection with {IdfDatabase} database. {ex}");
            }

            return conn;
        }
    }

    /// <summary>
    /// Gets or sets transaction used in database executions.
    /// </summary>
    /// <value>Database transaction.</value>
    public static DbTransaction Transaction { get; set; }

    /// <summary>
    /// Configure database options.
    /// </summary>
    public static void ConnectionFactory()
    {
        if (IdfDatabase == EnumDatabase.Oracle)
        {
            connection = new DbOracle();
        }
        else if (IdfDatabase == EnumDatabase.Sqlserver)
        {
            connection = new DbSqlServer();
        }
    }

    /// <summary>
    /// Load connection.
    /// </summary>
    /// <param name="connectionString">Connection string.</param>
    /// <param name="databaseType">Database type.</param>
    /// <returns>True: connection loaded | False: error.</returns>
    public static bool LoadConnection(string connectionString, EnumDatabase databaseType)
    {
        if (!string.IsNullOrEmpty(connectionString) && databaseType != EnumDatabase.Invalid)
        {
            ConnectionString = connectionString;
            IdfDatabase = databaseType;
        }
        else
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Dispose transaction propertie.
    /// </summary>
    public static void DisposeTransaction()
    {
        if (Transaction != null)
        {
            Transaction.Dispose();
            Transaction = null;
        }
    }
}
