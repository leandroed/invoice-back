using DbLib.Database;
using DbLib.Enumerators;
using DbLib.Models;
using InvoiceApi.Protocols;
using InvoiceApi.Vendor;
using Newtonsoft.Json;
using Serilog;

namespace InvoiceApi;

/// <summary>
/// Database configuration class.
/// </summary>
public class DatabaseConfig
{
    private readonly ISecretManager secretManager;
    private readonly bool isCloudEnv;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConfig"/> class.
    /// </summary>
    /// <param name="isCloudEnv">Identifies if app running on cloud environment.</param>
    public DatabaseConfig(bool isCloudEnv)
    {
        this.secretManager = new AwsSecretManager();
        this.isCloudEnv = isCloudEnv;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConfig"/> class.
    /// </summary>
    /// <param name="secretManager">Secret manager.</param>
    /// <param name="isCloudEnv">Identifies if app running on cloud environment.</param>
    public DatabaseConfig(ISecretManager secretManager, bool isCloudEnv)
    {
        this.secretManager = secretManager;
        this.isCloudEnv = isCloudEnv;
    }

    /// <summary>
    /// Start database connection.
    /// </summary>
    /// <returns>True: success | False: error.</returns>
    public bool StartConnection()
    {
        try
        {
            string connectionString = string.Empty;

            if (this.isCloudEnv)
            {
                string secretDb = this.secretManager.Get();
                Credentials credentials = JsonConvert.DeserializeObject<Credentials>(secretDb);
                DatabaseConnectionString connectionStringBuilder = new SqlServerConnectionString();

                if (!connectionStringBuilder.ValidateProperties(credentials))
                {
                    Log.Error("Error the received credentials doesn't match with expected.");
                    return false;
                }

                connectionString = connectionStringBuilder.CreateStringConnection(credentials);

                if (string.IsNullOrEmpty(connectionString) || credentials.Engine != "sqlserver")
                {
                    Log.Error("Error the connection string is invalid.");
                    return false;
                }
            }
            else
            {
                connectionString = this.secretManager.SecretName;
            }

            if (!Connection.LoadConnection(connectionString, EnumDatabase.Sqlserver) && Connection.Conn == null)
            {
                Log.Error("Error it's not possible establish connection with database.");
                return false;
            }

            _ = Connection.Conn;
        }
        catch (Exception ex)
        {
            Log.Error($"Error exception when initializing the database connection. {ex.Message}");
            return false;
        }

        return true;
    }
}
