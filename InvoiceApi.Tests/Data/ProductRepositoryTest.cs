using System;
using System.Collections.Generic;
using System.Data.Common;
using DbLib.Database;
using DbLib.Enumerators;
using InvoiceApi.Data;
using NSubstitute;
using Xunit;

namespace InvoiceApi.Tests;

/// <summary>
/// Product repository test.
/// </summary>
public class ProductRepositoryTest
{
    private readonly InvoiceMock invContentMock;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProductRepositoryTest"/> class.
    /// </summary>
    public ProductRepositoryTest()
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
        ProductRepository productRepository = new ProductRepository();
        bool result = productRepository.Insert(null);

        Assert.True(result);
    }

    /// <summary>
    /// Test insert products returns false on insert fails.
    /// </summary>
    [Fact]
    public void TestInsertFailsRetunsFalse()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(true, false);
        connCommMock.ExecuteParametrizedQuery(Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(false);

        ProductRepository prodRepository = new ProductRepository(connCommMock);
        bool result = prodRepository.Insert(this.invContentMock.InvoiceContentMock);

        Assert.False(result);
    }

    /// <summary>
    /// Test insert product returns false on exception.
    /// </summary>
    [Fact]
    public void TestInsertReturnsFalseOnException()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(x => throw new Exception());

        ProductRepository prodRepository = new ProductRepository(connCommMock);
        bool result = prodRepository.Insert(this.invContentMock.InvoiceContentMock);

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

        ProductRepository prodRepository = new ProductRepository(connCommMock);
        bool result = prodRepository.Insert(this.invContentMock.InvoiceContentMock);

        Assert.True(result);
    }

    /// <summary>
    /// Test select brands returns empty list when a null data reader is returned.
    /// </summary>
    [Fact]
    public void TestSelectBrandRetursEmptyNullDataReader()
    {
        DbDataReader readerMock = null;
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(readerMock);

        ProductRepository prodRepository = new ProductRepository(connCommMock);
        List<string> result = prodRepository.GetBrands();

        Assert.True(result.Count == 0);
    }

    /// <summary>
    /// Test select brands returns empty list when an exception occurs.
    /// </summary>
    [Fact]
    public void TestSelectBrandRetursEmptyException()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(x => throw new Exception());

        ProductRepository prodRepository = new ProductRepository(connCommMock);
        List<string> result = prodRepository.GetBrands();

        Assert.True(result.Count == 0);
    }

    /// <summary>
    /// Test select brands returns brands on success.
    /// </summary>
    [Fact]
    public void TestSelectBrandRetursBrandSuccess()
    {
        DbDataReader readerMock = Substitute.For<DbDataReader>();
        readerMock.Read().Returns(true, true, false);
        readerMock.GetString(0).Returns("Brand1", "Brand2");

        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteReader(Arg.Any<string>()).Returns(readerMock);

        ProductRepository prodRepository = new ProductRepository(connCommMock);
        List<string> result = prodRepository.GetBrands();

        Assert.True(result.Count == 2);
        Assert.True(result.Contains("Brand1"));
    }
}
