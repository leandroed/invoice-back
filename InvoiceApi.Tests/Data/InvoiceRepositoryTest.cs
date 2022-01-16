using System;
using System.Collections.Generic;
using DbLib.Database;
using DbLib.Enumerators;
using InvoiceApi.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Xunit;

namespace InvoiceApi.Tests;

/// <summary>
/// Test invoice repository.
/// </summary>
public class InvoiceRepositoryTest
{
    private JArray mockContent;

    /// <summary>
    /// Initializes a new instance of the <see cref="InvoiceRepositoryTest"/> class.
    /// </summary>
    public InvoiceRepositoryTest()
    {
        this.mockContent = JsonConvert.DeserializeObject<JArray>("[{\"id\": \"anyId\", \"nfeProc\": \"anyContent\"}]");
        Connection.LoadConnection("Data Source=host;Initial Catalog=mydb;Integrated Security=false;User ID=user;Password=pass;MultipleActiveResultSets=True;", EnumDatabase.Sqlserver);
        _ = Connection.Conn;
    }

    /// <summary>
    /// Test insert original content returns true when received no content to insert.
    /// </summary>
    [Fact]
    public void TestInsertOriginalContentNullInvoicesReturnsTrue()
    {
        InvoiceRepository invoiceRepository = new InvoiceRepository();
        bool result = invoiceRepository.InsertOriginalContent(null);
        Assert.True(result);
    }

    /// <summary>
    /// Test insert original content returns false when received content with unexpected format.
    /// </summary>
    [Fact]
    public void TestInsertOriginalContentInvalidContentReturnsFalse()
    {
        InvoiceRepository invoiceRepository = new InvoiceRepository();
        JArray mockContent = JsonConvert.DeserializeObject<JArray>("[{\"anyKey\": \"anyValue\"}]");
        bool result = invoiceRepository.InsertOriginalContent(mockContent);
        Assert.False(result);
    }

    /// <summary>
    /// Test insert original content returns true register exists
    /// </summary>
    [Fact]
    public void TestInsertOriginalContentReturnsTrueRegisterExists()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(true);

        IConnectionCommandsFactory connCommFactMock = Substitute.For<IConnectionCommandsFactory>();
        connCommFactMock.Create().Returns(connCommMock);

        InvoiceRepository invoiceRepository = new InvoiceRepository(connCommFactMock);
        bool result = invoiceRepository.InsertOriginalContent(this.mockContent);
        Assert.True(result);
    }

    /// <summary>
    /// Test insert original content returns false on insert error.
    /// </summary>
    [Fact]
    public void TestInsertOriginalContentReturnsFalseOnInsertError()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(false);
        connCommMock.ExecuteParametrizedQuery(Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(false);

        IConnectionCommandsFactory connCommFactMock = Substitute.For<IConnectionCommandsFactory>();
        connCommFactMock.Create().Returns(connCommMock);

        InvoiceRepository invoiceRepository = new InvoiceRepository(connCommFactMock);
        bool result = invoiceRepository.InsertOriginalContent(this.mockContent);
        Assert.False(result);
    }

    /// <summary>
    /// Test insert original content returns false on exception.
    /// </summary>
    [Fact]
    public void TestInsertOriginalContentReturnsFalseOnException()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(x => throw new Exception());

        IConnectionCommandsFactory connCommFactMock = Substitute.For<IConnectionCommandsFactory>();
        connCommFactMock.Create().Returns(connCommMock);

        InvoiceRepository invoiceRepository = new InvoiceRepository(connCommFactMock);
        bool result = invoiceRepository.InsertOriginalContent(this.mockContent);
        Assert.False(result);
    }

    /// <summary>
    /// Test insert original content returns True on insert success.
    /// </summary>
    [Fact]
    public void TestInsertOriginalContentReturnsTrueOnSuccess()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.HasRegister(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(false);
        connCommMock.ExecuteParametrizedQuery(Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(true);

        IConnectionCommandsFactory connCommFactMock = Substitute.For<IConnectionCommandsFactory>();
        connCommFactMock.Create().Returns(connCommMock);

        InvoiceRepository invoiceRepository = new InvoiceRepository(connCommFactMock);
        bool result = invoiceRepository.InsertOriginalContent(this.mockContent);
        Assert.True(result);
    }

    /// <summary>
    /// Test add altered data returns true when received no content to insert.
    /// </summary>
    [Fact]
    public void TestAddAlteredDataNullInvoicesReturnsTrue()
    {
        InvoiceRepository invoiceRepository = new InvoiceRepository();
        bool result = invoiceRepository.AddAlteredData(null);
        Assert.True(result);
    }

    /// <summary>
    /// Test add altered data returns true on invalid invoices received.
    /// </summary>
    [Fact]
    public void TestAddAlteredDataInvalidContentReturnsFalse()
    {
        InvoiceRepository invoiceRepository = new InvoiceRepository();
        JArray mockContent = JsonConvert.DeserializeObject<JArray>("[{\"anyKey\": \"anyValue\"}]");
        bool result = invoiceRepository.AddAlteredData(mockContent);
        Assert.False(result);
    }

    /// <summary>
    /// Test insert original content returns false on update error.
    /// </summary>
    [Fact]
    public void TestAddAlteredDataReturnsFalseOnUpdateError()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteParametrizedQuery(Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(false);

        IConnectionCommandsFactory connCommFactMock = Substitute.For<IConnectionCommandsFactory>();
        connCommFactMock.Create().Returns(connCommMock);

        InvoiceRepository invoiceRepository = new InvoiceRepository(connCommFactMock);
        bool result = invoiceRepository.AddAlteredData(this.mockContent);
        Assert.False(result);
    }

    /// <summary>
    /// Test add altered data returns false on exception.
    /// </summary>
    [Fact]
    public void TestAddAlteredDataReturnsFalseOnException()
    {
        IConnectionCommands connCommMock = Substitute.For<IConnectionCommands>();
        connCommMock.ExecuteParametrizedQuery(Arg.Any<string>(), Arg.Any<List<Parameters>>()).Returns(x => throw new Exception());

        IConnectionCommandsFactory connCommFactMock = Substitute.For<IConnectionCommandsFactory>();
        connCommFactMock.Create().Returns(connCommMock);

        InvoiceRepository invoiceRepository = new InvoiceRepository(connCommFactMock);
        bool result = invoiceRepository.AddAlteredData(this.mockContent);
        Assert.False(result);
    }
}
