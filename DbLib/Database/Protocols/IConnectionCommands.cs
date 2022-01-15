using System.Data.Common;

namespace DbLib.Database;

/// <summary>
/// Connection commands interface.
/// </summary>
public interface IConnectionCommands : IDisposable
{
    /// <summary>
    /// Gets or sets database transaction.
    /// </summary>
    /// <value>Database transaction.</value>
    DbTransaction Transaction { get; set; }

    /// <summary>
    /// Execute query.
    /// </summary>
    /// <param name="query">Query.</param>
    /// <returns>True: success | False: error.</returns>
    bool ExecuteQuery(string query);

    /// <summary>
    /// Execute query parametrized.
    /// </summary>
    /// <param name="query">Sql query.</param>
    /// <param name="parameters">Parameters.</param>
    /// <returns>True: success | False: error.</returns>
    bool ExecuteParametrizedQuery(string query, ICollection<Parameters> parameters);

    /// <summary>
    /// Execute query in datareader using parameters.
    /// </summary>
    /// <param name="query">Sql query.</param>
    /// <param name="parameters">List of parameters.</param>
    /// <returns>Data reader with results.</returns>
    DbDataReader ExecuteParametrizedReader(string query, ICollection<Parameters> parameters);

    /// <summary>
    /// Execute query in datareader.
    /// </summary>
    /// <param name="query">Sql query.</param>
    /// <returns>Database reader.</returns>
    DbDataReader ExecuteReader(string query);

    /// <summary>
    /// Checks if the register already exists in database table.
    /// </summary>
    /// <param name="table">Table name.</param>
    /// <param name="where">Where filter.</param>
    /// <param name="dbParams">Database params.</param>
    /// <returns>True: Exists | False: Not exists.</returns>
    bool HasRegister(string table, string where, ICollection<Parameters> dbParams);

    /// <summary>
    /// Begin database transaction.
    /// </summary>
    void BeginTransaction();

    /// <summary>
    /// Commit the transaction and dispose.
    /// </summary>
    void CommitTransaction();

    /// <summary>
    /// Rollback the transaction and dispose.
    /// </summary>
    void RollbackTransaction();

    /// <summary>
    /// Dispose transaction propertie.
    /// </summary>
    void DisposeTransaction();
}
