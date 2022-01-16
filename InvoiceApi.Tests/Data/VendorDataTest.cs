using System;
using System.Net.Http;
using System.Threading.Tasks;
using InvoiceApi.Data;
using InvoiceApi.Protocols;
using NSubstitute;
using Xunit;

namespace InvoiceApi.Tests;

/// <summary>
/// Vendor data test.
/// </summary>
public class VendorDataTest
{
    /// <summary>
    /// Test receive data returns empty when has invalid url.
    /// </summary>
    [Fact]
    public void TestReceiveDataReturnsEmptyInvalidUrl()
    {
        VendorData vendorData = new VendorData("", null);
        string result = vendorData.ReceiveData();

        Assert.True(string.IsNullOrEmpty(result));
    }

    /// <summary>
    /// Test receive data returns empty on exception.
    /// </summary>
    [Fact]
    public void TestReceiveDataReturnsEmptyOnException()
    {
        IHttpCliFactory cliFactoryMock = Substitute.For<IHttpCliFactory>();
        cliFactoryMock.Create().Returns(x => throw new Exception());

        VendorData vendorData = new VendorData("http://google.com", cliFactoryMock);
        string result = vendorData.ReceiveData();

        Assert.True(string.IsNullOrEmpty(result));
    }
}
