using System.Data;
using System.Data.Common;
using DbLib.Database;
using InvoiceApi.Models;
using Serilog;

namespace InvoiceApi.Data;

/// <summary>
/// Product repository class.
/// </summary>
public class ProductRepository
{
    private readonly IConnectionCommands connCommands;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    /// <param name="connCommands">Connection commands.</param>
    public ProductRepository(IConnectionCommands connCommands)
    {
        this.connCommands = connCommands;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepository"/> class.
    /// </summary>
    public ProductRepository()
    {
        this.connCommands = new ConnectionCommands(Connection.Conn);
    }

    /// <summary>
    /// Insert the products in invoice.
    /// </summary>
    /// <param name="content">Invoices.</param>
    /// <returns>True: success | False: error.</returns>
    public bool Insert(InvoiceContent content)
    {
        if (content == null)
        {
            Log.Information("Empty invoices received, no products to insert.");
            return true;
        }

        try
        {
            foreach (Det det in content.NfeProc.NFe.InfNfe.Det)
            {
                string productCode = det.Prod.CProd;
                string product = det.Prod.XProd;
                string brand = det.Prod.Marca;

                string queryInsert = $"INSERT INTO PRODUCTS (CDPROD, DESCRIPTION, BRAND) VALUES ({Connection.PrefixParam}cdprod, {Connection.PrefixParam}description, {Connection.PrefixParam}brand)";

                List<Parameters> parameters = new List<Parameters>
                {
                    new Parameters { ParameterName = $"{Connection.PrefixParam}cdprod", Value = productCode, DbType = DbType.String },
                    new Parameters { ParameterName = $"{Connection.PrefixParam}description", Value = product, DbType = DbType.String },
                    new Parameters { ParameterName = $"{Connection.PrefixParam}brand", Value = brand, DbType = DbType.String },
                };

                if (this.connCommands.HasRegister("PRODUCTS", $"CDPROD = {Connection.PrefixParam}cdprod", parameters))
                {
                    Log.Debug("The product already exists in product table.");
                    continue;
                }

                if (!this.connCommands.ExecuteParametrizedQuery(queryInsert, parameters))
                {
                    Log.Error("Error when trying to insert a product.");
                    return false;
                }
            }
        }
        catch (Exception ex)
        {
            Log.Error($"Error fault when trying to insert the product. {ex.Message}");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Get brand list.
    /// </summary>
    public List<string> GetBrands()
    {
        List<string> brands = new List<string>();

        try
        {
            string query = "SELECT DISTINCT BRAND FROM PRODUCTS";
            DbDataReader reader = this.connCommands.ExecuteReader(query);

            if (reader == null)
            {
                Log.Information("A null data reader received when getting the brands.");
                return brands;
            }

            while (reader.Read())
            {
                brands.Add(reader.GetString(0));
            }

            reader.DisposeAsync();
            reader.Close();
        }
        catch (Exception ex)
        {
            Log.Error($"Error fault when getting the brands in database. {ex.Message}");
            return new List<string>();
        }

        return brands;
    }
}
