using System;
using System.Collections.Generic;
using DbLib.Database;
using DbLib.Enumerators;
using InvoiceApi.Data;
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
}
