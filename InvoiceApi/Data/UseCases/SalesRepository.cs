using System.Data;
using DbLib.Database;
using InvoiceApi.Models;
using Serilog;

namespace InvoiceApi.Data;

/// <summary>
/// Sales repository.
/// </summary>
public class SalesRepository
{
    private IConnectionCommands connCommands;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesRepository"/> class.
    /// </summary>
    public SalesRepository()
    {
        this.connCommands = new ConnectionCommands(Connection.Conn);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesRepository"/> class.
    /// </summary>
    /// <param name="connCommands">Connection commands.</param>
    public SalesRepository(IConnectionCommands connCommands)
    {
        this.connCommands = connCommands;
    }

    /// <summary>
    /// Insert sales from invoices in database.
    /// </summary>
    /// <param name="content">Invoice content.</param>
    /// <returns>True: success | False: error.</returns>
    public bool Insert(InvoiceContent content)
    {
        System.Globalization.CultureInfo customCulture = (System.Globalization.CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
        customCulture.NumberFormat.NumberDecimalSeparator = ".";
        System.Threading.Thread.CurrentThread.CurrentCulture = customCulture;

        if (content == null)
        {
            Log.Information("Empty invoices received, no sales to insert.");
            return true;
        }

        try
        {
            foreach (Det det in content.NfeProc.NFe.InfNfe.Det)
            {
                DateTimeOffset.TryParse(content.NfeProc.NFe.InfNfe.Ide.DhEmi, out DateTimeOffset dtemi);
                decimal.TryParse(det.Prod.Preco, out decimal price);

                List<Parameters> parameters = new List<Parameters>
                {
                    new Parameters { ParameterName = $"{Connection.PrefixParam}id", Value = det.Prod.CEAN, DbType = DbType.String },
                };

                if (this.connCommands.HasRegister("SALES", $"ID = {Connection.PrefixParam}id", parameters))
                {
                    Log.Debug("The sale already exists in sales table.");
                    continue;
                }

                string query = $@"INSERT INTO SALES (ID, CDPROD, PRICE, UCOM, VUNCOM, VPROD, UTRIB, QTRIB, VUNTRIB, TAX, DTEMIT) VALUES (
                    {Connection.PrefixParam}id, {Connection.PrefixParam}cdprod, {Connection.PrefixParam}price, {Connection.PrefixParam}ucom, {Connection.PrefixParam}vuncom,
                    {Connection.PrefixParam}vprod, {Connection.PrefixParam}utrib, {Connection.PrefixParam}qtrib, {Connection.PrefixParam}vuntrib, {Connection.PrefixParam}tax, 
                    {Connection.PrefixParam}dtemi)";

                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}cdprod", Value = det.Prod.CProd, DbType = DbType.String });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}price", Value = price, DbType = DbType.Decimal });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}ucom", Value = det.Prod.UCom, DbType = DbType.String });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}vuncom", Value = det.Prod.VUnCom, DbType = DbType.Decimal });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}vprod", Value = det.Prod.VProd, DbType = DbType.Decimal });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}utrib", Value = det.Prod.UTrib, DbType = DbType.String });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}qtrib", Value = det.Prod.QTrib, DbType = DbType.Decimal });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}vuntrib", Value = det.Prod.VUnTrib, DbType = DbType.Decimal });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}tax", Value = det.Prod.Imposto, DbType = DbType.Decimal });
                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}dtemi", Value = dtemi, DbType = DbType.DateTimeOffset });

                if (!this.connCommands.ExecuteParametrizedQuery(query, parameters))
                {
                    Log.Information("Error when trying to insert a sale.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error fault when trying to insert sales. {ex.Message}");
            return false;
        }

        return true;
    }
}
