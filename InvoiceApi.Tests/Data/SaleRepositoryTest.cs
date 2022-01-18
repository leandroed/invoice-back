using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using DbLib.Database;
using DbLib.Enumerators;
using InvoiceApi.Data;
using InvoiceApi.Models;
using NSubstitute;
using Xunit;

namespace InvoiceApi.Tests;

/// <summary>
/// Sale repository test.
/// </summary>
public class SalesRepositoryTest
{
    private readonly InvoiceMock invContentMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="SalesRepositoryTest"/> class.
    /// </summary>
    public SalesRepositoryTest()
    {
        this.invContentMock = new InvoiceMock();
        Connection.LoadConnection("Data Source=host;Initial Catalog=mydb;Integrated Security=false;User ID=user;Password=pass;MultipleActiveResultSets=True;", EnumDatabase.Sqlserver);
        _ = Connection.Conn;
    }

    /// <summary>
    /// Test no when has no content to insert returns true.
    /// </summary>
    [Fact]
    public void TestInsertNullContentReturnsTrue()
    {
        SalesRepository salesRepository = new SalesRepository();
        bool result = salesRepository.Insert(null);

        Assert.True(result);
    }

    /// <summary>
    /// Test insert sales returns false on insert fails.
    /// </summary>
    [Fact]
    public void TestInsertFailsRetunsFalse()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(true, false);
        connCommMock.ExecuteParametrizedQuery(Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(false);

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        bool result = saleRepository.Insert(this.invContentMock.InvoiceContentMock);

        Assert.False(result);
    }

    /// <summary>
    /// Test insert sale returns false on exception.
    /// </summary>
    [Fact]
    public void TestInsertReturnsFalseOnException()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(x => throw new Exception());

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        bool result = saleRepository.Insert(this.invContentMock.InvoiceContentMock);

        Assert.False(result);
    }

    /// <summary>
    /// Test insert returns success.
    /// </summary>
    [Fact]
    public void TestInsertSuccess()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(false);
        connCommMock.ExecuteParametrizedQuery(Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(true);

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        bool result = saleRepository.Insert(this.invContentMock.InvoiceContentMock);

        Assert.True(result);
    }

    /// <summary>
    /// Test list sales by brand returns empty list when receive a null reader.
    /// </summary>
    [Fact]
    public void TestListSalesByBrandEmptyListNullReader()
    {
        DbDataReader dataReaderMock = null;
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(dataReaderMock);

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        List<SaleByBrand> salesByBrand = saleRepository.ListSalesByBrand();

        Assert.True(salesByBrand.Count == 0);
    }

    /// <summary>
    /// Test list sales by brand returns empty list on exception.
    /// </summary>
    [Fact]
    public void TestListSalesByBrandEmptyListOnException()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(x => throw new Exception());

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        List<SaleByBrand> salesByBrand = saleRepository.ListSalesByBrand();

        Assert.True(salesByBrand.Count == 0);
    }

    /// <summary>
    /// Test list sales by brand returns list with content on success.
    /// </summary>
    [Fact]
    public void TestListSalesByBrandReturnsAListOnSuccess()
    {
        DbDataReader dataReaderMock = Substitute.For<DbDataReader>();
        dataReaderMock.Read().Returns(true, false);
        dataReaderMock.GetString(0).Returns("1");
        dataReaderMock.GetDecimal(1).Returns(2);
        dataReaderMock.GetString(2).Returns("Brand1");

        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(dataReaderMock);

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        List<SaleByBrand> salesByBrand = saleRepository.ListSalesByBrand();

        Assert.True(salesByBrand.Count > 0);
        Assert.True(salesByBrand.First().Brand == "Brand1");
    }

    /// <summary>
    /// Test list sales return empty list when receive an empty reader.
    /// </summary>
    [Fact]
    public void TestListSalesEmptyListNullReader()
    {
        DbDataReader dataReaderMock = null;
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(dataReaderMock);

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        List<Sale> sales = saleRepository.ListSales();

        Assert.True(sales.Count == 0);
    }

    /// <summary>
    /// Test list salesreturns empty list on exception.
    /// </summary>
    [Fact]
    public void TestListSalesEmptyListOnException()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(x => throw new Exception());

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        List<Sale> sales = saleRepository.ListSales();

        Assert.True(sales.Count == 0);
    }

    /// <summary>
    /// Test list sale returns list with content on success.
    /// </summary>
    [Fact]
    public void TestListSaleReturnsAListOnSuccess()
    {
        DbDataReader dataReaderMock = Substitute.For<DbDataReader>();
        dataReaderMock.Read().Returns(true, false);
        dataReaderMock.GetString(0).Returns("1");
        dataReaderMock.GetDecimal(1).Returns(2);
        dataReaderMock.GetString(2).Returns("value2");
        dataReaderMock.GetDecimal(3).Returns(2);
        dataReaderMock.GetDecimal(4).Returns(2);
        dataReaderMock.GetString(5).Returns("value5");
        dataReaderMock.GetDecimal(6).Returns(2);
        dataReaderMock.GetDecimal(7).Returns(2);
        dataReaderMock.GetDecimal(8).Returns(2);
        dataReaderMock.GetDateTime(9).Returns(DateTime.Now);
        dataReaderMock.GetString(10).Returns("Product1");
        dataReaderMock.GetString(11).Returns("value11");
        dataReaderMock.GetDateTime(12).Returns(DateTime.Now);

        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(dataReaderMock);

        SalesRepository saleRepository = new SalesRepository(connCommMock);
        List<Sale> sale = saleRepository.ListSales();

        Assert.True(sale.Count > 0);
        Assert.True(sale.First().Product == "Product1");
    }
}
