using System.Data;
using System.Data.Common;
using DbLib.Database;
using InvoiceApi.Models;
using Serilog;

namespace InvoiceApi.Data;

/// <summary>
/// Sales repository.
/// </summary>
public class SalesRepository
{
    private readonly IConnectionCommands connCommands;

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

                string query = $@"INSERT INTO SALES (ID, IDINV, CDPROD, PRICE, UCOM, VUNCOM, VPROD, UTRIB, QTRIB, VUNTRIB, TAX, DTEMIT) VALUES (
                    {Connection.PrefixParam}id, {Connection.PrefixParam}idinv, {Connection.PrefixParam}cdprod, {Connection.PrefixParam}price, {Connection.PrefixParam}ucom, {Connection.PrefixParam}vuncom,
                    {Connection.PrefixParam}vprod, {Connection.PrefixParam}utrib, {Connection.PrefixParam}qtrib, {Connection.PrefixParam}vuntrib, {Connection.PrefixParam}tax, 
                    {Connection.PrefixParam}dtemi)";

                parameters.Add(new Parameters { ParameterName = $"{Connection.PrefixParam}idinv", Value = content.Id, DbType = DbType.String });
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

    /// <summary>
    /// List the sales by branch.
    /// </summary>
    /// <returns>Sale by branch.</returns>
    public List<SaleByBrand> ListSalesByBrand()
    {
        List<SaleByBrand> saleByBrands = new List<SaleByBrand>();

        try
        {
            string query = $@"Select 
                            S.ID,
                            S.PRICE,
                            P.BRAND 
                        FROM SALES S
                        INNER JOIN PRODUCTS P ON S.CDPROD = P.CDPROD ";

            DbDataReader reader = this.connCommands.ExecuteReader(query);

            if (reader == null)
            {
                Log.Information("A null data reader received when getting the sales.");
                return saleByBrands;
            }

            while (reader.Read())
            {
                SaleByBrand salebybrand = new SaleByBrand
                {
                    Id = reader.GetString(0),
                    Price = reader.GetDecimal(1),
                    Brand = reader.GetString(2),
                };
                saleByBrands.Add(salebybrand);
            }

            reader.DisposeAsync();
            reader.Close();
        }
        catch (Exception ex)
        {
            Log.Error($"Error fault when trying to get the list of sales by brand. {ex.Message}");
            return new List<SaleByBrand>();
        }

        return saleByBrands;
    }

    /// <summary>
    /// List the sales in database.
    /// </summary>
    /// <returns>Sales list.</returns>
    public List<Sale> ListSales()
    {
        List<Sale> sales = new List<Sale>();

        try
        {
            string query = $@"SELECT S.ID, S.PRICE, S.UCOM, S.VUNCOM, S.VPROD, S.UTRIB, S.QTRIB, S.VUNTRIB, S.TAX, S.DTEMIT, 
                                    P.DESCRIPTION, P.BRAND, 
                                    I.DHEMI 
                            FROM SALES S 
                                INNER JOIN PRODUCTS P ON S.CDPROD = P.CDPROD
                                INNER JOIN INVOICES I ON I.ID = S.IDINV";

            DbDataReader reader = this.connCommands.ExecuteReader(query);

            if (reader == null)
            {
                Log.Information("A null data reader received when getting the sales.");
                return sales;
            }

            while (reader.Read())
            {
                Sale sale = new Sale
                {
                    Id = reader.GetString(0),
                    Price = reader.GetDecimal(1),
                    Ucom = reader.GetString(2),
                    Vuncom = reader.GetDecimal(3),
                    Vprod = reader.GetDecimal(4),
                    Utrib = reader.GetString(5),
                    Qtrib = reader.GetDecimal(6),
                    Vuntrib = reader.GetDecimal(7),
                    Tax = reader.GetDecimal(8),
                    Dtemi = reader.GetDateTime(9),
                    Product = reader.GetString(10),
                    Brand = reader.GetString(11),
                    Dhemi = reader.GetDateTime(12),
                };
                sales.Add(sale);
            }

            reader.DisposeAsync();
            reader.Close();
        }
        catch (Exception ex)
        {
            Log.Error($"Error when trying to get the sales. {ex.Message}");
            return new List<Sale>();
        }

        return sales;
    }
}
