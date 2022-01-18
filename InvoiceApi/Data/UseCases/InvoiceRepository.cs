using System.Data;
using DbLib.Database;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;

namespace InvoiceApi.Data;

/// <summary>
/// Invoice repository class.
/// </summary>
public class InvoiceRepository
{
    private readonly IConnectionCommandsFactory connFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceRepository"/> class.
    /// </summary>
    /// <param name="connFactory">Connection factory.</param>
    public InvoiceRepository(IConnectionCommandsFactory connFactory)
    {
        this.connFactory = connFactory;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceRepository"/> class.
    /// </summary>
    public InvoiceRepository()
    {
        this.connFactory = new ConnectionCommandsFactory();
    }

    /// <summary>
    /// Insert original invoices data in database.
    /// </summary>
    /// <param name="invoices">Invoices.</param>
    /// <returns>True: success | False: erro.</returns>
    public bool InsertOriginalContent(JArray invoices)
    {
        if (invoices == null)
        {
            Log.Information("The received invoices has no content.");
            return true;
        }

        try
        {
            foreach (JObject item in invoices)
            {
                if (!item.ContainsKey("id") && !item.ContainsKey("nfeProc"))
                {
                    Log.Error("Error the received nfe doesn't contains the expected format.");
                    return false;
                }

                string id = item["id"].ToString();
                string content = JsonConvert.SerializeObject(item["nfeProc"]);
                string dhemiString = item["nfeProc"]["NFe"]["infNFe"]["ide"]["dhEmi"].ToString();
                DateTimeOffset.TryParse(dhemiString, out DateTimeOffset dhemi);

                string query = $"INSERT INTO INVOICES (ID, ORIGINALCONTENT, DHEMI) VALUES ({Connection.PrefixParam}id, {Connection.PrefixParam}content, {Connection.PrefixParam}dhemi)";
                List<Parameters> parameters = new List<Parameters>
                {
                    new Parameters { ParameterName = $"{Connection.PrefixParam}id", Value = id, DbType = DbType.String },
                    new Parameters { ParameterName = $"{Connection.PrefixParam}content", Value = content, DbType = DbType.String },
                    new Parameters { ParameterName = $"{Connection.PrefixParam}dhemi", Value = dhemi, DbType = DbType.DateTimeOffset },
                };

                using IConnectionCommands connCommands = this.connFactory.Create();

                if (connCommands.HasRegister("INVOICES", $"ID = {Connection.PrefixParam}id", parameters))
                {
                    Log.Debug($"The invoice already exists in database id: '{id}'.");
                    continue;
                }

                if (!connCommands.ExecuteParametrizedQuery(query, parameters))
                {
                    Log.Error("Error when inserting invoice in database.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error fault when trying to insert external invoices. {ex.Message}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Updates adding altered invoices in database.
    /// </summary>
    /// <param name="invoices">Original invoices.</param>
    /// <returns>True: success | False: error.</returns>
    public bool AddAlteredData(JArray invoices)
    {
        if (invoices == null)
        {
            Log.Information("The received invoices has no content.");
            return true;
        }

        try
        {
            foreach (JObject item in invoices)
            {
                if (!item.ContainsKey("id") && !item.ContainsKey("nfeProc"))
                {
                    Log.Error("The received nfe doesn't contains a expected format.");
                    return false;
                }

                string id = item["id"].ToString();
                string content = JsonConvert.SerializeObject(item["nfeProc"]);

                string query = $"UPDATE INVOICES SET CONTENT = {Connection.PrefixParam}content WHERE ID = {Connection.PrefixParam}id";
                List<Parameters> parameters = new List<Parameters>
                {
                    new Parameters { ParameterName = $"{Connection.PrefixParam}id", Value = id, DbType = DbType.String },
                    new Parameters { ParameterName = $"{Connection.PrefixParam}content", Value = content, DbType = DbType.String },
                };

                using IConnectionCommands connCommands = this.connFactory.Create();

                if (connCommands.HasRegister("INVOICES", $"ID = {Connection.PrefixParam}id AND (CONTENT IS NOT NULL OR CONTENT <> '')", parameters))
                {
                    Log.Debug($"The invoice already exists in database id: '{id}'.");
                    continue;
                }

                if (!connCommands.ExecuteParametrizedQuery(query, parameters))
                {
                    Log.Error("Error when updating an altered invoice in database.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error faul when trying to update the altered external invoices. {ex.Message}");
            return false;
        }

        return true;
    }
}
