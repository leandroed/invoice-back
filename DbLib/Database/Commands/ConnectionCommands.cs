using System.Data.Common;
using DbLib.Enumerators;
using Oracle.ManagedDataAccess.Client;
using Serilog;

namespace DbLib.Database;

/// <summary>
/// Common commands for execute queys in database.
/// </summary>
public class ConnectionCommands : IConnectionCommands
{
    private readonly DbConnection connection;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionCommands"/> class.
    /// </summary>
    /// <param name="connection">Database connection.</param>
    public ConnectionCommands(DbConnection connection)
    {
        this.connection = connection;
        this.Transaction = null;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ConnectionCommands"/> class.
    /// </summary>
    /// <param name="connection">Database connection.</param>
    /// <param name="transaction">Database transaction.</param>
    public ConnectionCommands(DbConnection connection, DbTransaction transaction)
    {
        this.connection = connection;
        this.Transaction = transaction;
    }

    /// <summary>
    /// Gets or sets database transaction.
    /// </summary>
    /// <value>Database transaction.</value>
    public DbTransaction Transaction { get; set; }

    /// <summary>
    /// Gets or sets the database command.
    /// </summary>
    /// <value>Database command.</value>
    public DbCommand Command { get; set; }

    /// <summary>
    /// Execute query.
    /// </summary>
    /// <param name="query">Query string.</param>
    /// <returns>True: Success | False: Error.</returns>
    public bool ExecuteQuery(string query)
    {
        if (this.connection != null)
        {
            try
            {
                Log.Debug($"Executing query: '{query}'");
                using DbCommand command = this.connection.CreateCommand();
                command.CommandText = query;
                command.Transaction = this.Transaction;
                command.CommandTimeout = 0;
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Log.Error($"Error it's not possible execute query. '{ex}'");
                return false;
            }
        }
        else
        {
            Log.Error("Error it's not possible execute query, the connection is invalid.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Execute query parametrized.
    /// </summary>
    /// <param name="query">Sql query.</param>
    /// <param name="parameters">Parameters.</param>
    /// <returns>True: success | False: error.</returns>
    public bool ExecuteParametrizedQuery(string query, ICollection<Parameters> parameters)
    {
        if (this.connection == null || parameters == null || parameters.Count <= 0)
        {
            Log.Error("Error it was not possible execute query, invalid connection.");
            return false;
        }

        try
        {
            Log.Debug(query + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(parameters));

            using DbCommand command = this.connection.CreateCommand();

            foreach (var param in parameters)
            {
                DbParameter parameter = command.CreateParameter();
                parameter.ParameterName = param.ParameterName;
                parameter.Value = param.Value;
                parameter.DbType = param.DbType;
                command.Parameters.Add(parameter);
            }

            command.CommandText = query;
            command.CommandTimeout = 0;
            command.Transaction = this.Transaction;
            if (Connection.IdfDatabase == EnumDatabase.Oracle)
            {
                ((OracleCommand)command).BindByName = true;
            }

            command.ExecuteNonQuery();
        }
        catch (Exception ex)
        {
            Log.Error($"Error it's not possible execute query. {ex.Message}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Execute query in datareader.
    /// </summary>
    /// <param name="query">Sql query.</param>
    /// <returns>Database reader.</returns>
    public DbDataReader ExecuteReader(string query)
    {
        DbDataReader result = null;
        if (this.connection != null)
        {
            try
            {
                Log.Debug($"Executing reader: {query}");
                this.Command = this.connection.CreateCommand();
                this.Command.CommandText = query;
                this.Command.Transaction = this.Transaction;
                result = this.Command.ExecuteReader();
            }
            catch (Exception ex)
            {
                Log.Error($"Error it's not possible get data reader for query. {ex}");
            }
        }
        else
        {
            Log.Error($"Error it's not possible execute query, the connection is invalid.");
        }

        return result;
    }

    /// <summary>
    /// Execute query in datareader using parameters.
    /// </summary>
    /// <param name="query">Sql query.</param>
    /// <param name="parameters">List of parameters.</param>
    /// <returns>Data reader with results.</returns>
    public DbDataReader ExecuteParametrizedReader(string query, ICollection<Parameters> parameters)
    {
        DbDataReader result = null;

        if (this.connection == null || parameters == null || parameters.Count <= 0)
        {
            Log.Error("Error it was not possible execute query, invalid connection.");
            return result;
        }

        try
        {
            Log.Debug(query + "\n" + Newtonsoft.Json.JsonConvert.SerializeObject(parameters));

            this.Command = this.connection.CreateCommand();

            foreach (var param in parameters)
            {
                DbParameter parameter = this.Command.CreateParameter();
                parameter.ParameterName = param.ParameterName;
                parameter.Value = param.Value;
                parameter.DbType = param.DbType;
                this.Command.Parameters.Add(parameter);
            }

            this.Command.CommandText = query;
            this.Command.Transaction = this.Transaction;
            result = this.Command.ExecuteReader();
        }
        catch (Exception ex)
        {
            Log.Error($"Error it's not possible get data reader for query. {ex}");
        }

        return result;
    }

    /// <summary>
    /// Checks if the register already exists in database table.
    /// </summary>
    /// <param name="table">Table name.</param>
    /// <param name="where">Where filter.</param>
    /// <param name="dbParams">Database params.</param>
    /// <returns>True: Exists | False: Not exists.</returns>
    public bool HasRegister(string table, string where, ICollection<Parameters> dbParams)
    {
        if (string.IsNullOrEmpty(where))
        {
            throw new ArgumentException($"Error when getting the primary key to '{table}', invalid filter received.");
        }

        string checkQuery = $"SELECT COUNT(1) HASDATA FROM {table} WHERE {where}";
        DbDataReader dataReader = this.ExecuteParametrizedReader(checkQuery, dbParams);

        bool hasData = false;
        if (dataReader != null && dataReader.Read())
        {
            hasData = dataReader.GetInt32(0) > 0;
            dataReader.Close();
            dataReader.DisposeAsync();
        }

        this.Dispose();

        return hasData;
    }

    /// <summary>
    /// Begin transaction.
    /// </summary>
    public void BeginTransaction()
    {
        Log.Debug("BeginTransaction");
        this.Transaction = this.connection.BeginTransaction();
    }

    /// <summary>
    /// Commit a transaction and dispose.
    /// </summary>
    public void CommitTransaction()
    {
        Log.Debug("CommitTransaction");
        
        this.Transaction.Commit();
        this.DisposeTransaction();
    }

    /// <summary>
    /// Rollback the transaction and dispose.
    /// </summary>
    public void RollbackTransaction()
    {
        Log.Debug("RollbackTransaction");
        
        this.Transaction.Dispose();
        this.DisposeTransaction();
    }

    /// <summary>
    /// Dispose transaction propertie.
    /// </summary>
    public void DisposeTransaction()
    {
        if (this.Transaction != null)
        {
            this.Transaction.Dispose();
            this.Transaction = null;
        }
    }

    /// <summary>
    /// Dispose class components.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Dispose class components.
    /// </summary>
    /// <param name="disposing">True: dispose | false: not dispose.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (this.Command != null)
        {
            this.Command.Dispose();
            this.Command = null;
        }
    }
}
