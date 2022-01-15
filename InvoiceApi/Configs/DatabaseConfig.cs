using DbLib.Database;
using DbLib.Enumerators;
using DbLib.Models;
using InvoiceApi.Vendor;
using Newtonsoft.Json;
using Serilog;

namespace InvoiceApi;

public class DatabaseConfig
{
    private ISecretManager secretManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConfig"/> class.
    /// </summary>
    public DatabaseConfig()
    {
        this.secretManager = new AwsSecretManager();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseConfig"/> class.
    /// </summary>
    /// <param name="secretManager">Secret manager.</param>
    public DatabaseConfig(ISecretManager secretManager)
    {
        this.secretManager = secretManager;
    }

    /// <summary>
    /// Start database connection.
    /// </summary>
    /// <returns>True: success | False: error.</returns>
    public bool StartConnection()
    {
        try
        {
            string secretDb = this.secretManager.Get();
            // string secretDb = "{\"username\":\"proleandro\",\"password\":\"cob3rtor\",\"engine\":\"sqlserver\",\"host\":\"invoice-db.c3p9sdnpgsml.us-east-2.rds.amazonaws.com\",\"port\":\"1433\",\"dbname\":\"invoicedb\"}";
            Credentials credentials = JsonConvert.DeserializeObject<Credentials>(secretDb);
            DatabaseConnectionString connectionStringBuilder = new SqlServerConnectionString();

            if (!connectionStringBuilder.ValidateProperties(credentials))
            {
                Log.Error("Error the received credentials doesn't match with expected.");
                return false;
            }

            string connectionString = connectionStringBuilder.CreateStringConnection(credentials);

            if (string.IsNullOrEmpty(connectionString) || credentials.Engine != "sqlserver")
            {
                Log.Error("Error the connection string is invalid.");
                return false;
            }

            if (!Connection.LoadConnection(connectionString, EnumDatabase.Sqlserver) && Connection.Conn == null)
            {
                Log.Error("Error it's not possible establish connection with database.");
                return false;
            }
        }
        catch(Exception ex)
        {
            Log.Error($"Error exception when initializing the database connection. {ex.Message}");
            return false;
        }

        return true;
    }
}
