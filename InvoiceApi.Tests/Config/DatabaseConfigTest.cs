using System;
using NSubstitute;
using Xunit;

namespace InvoiceApi.Tests;

public class DatabaseConfigTest
{
    private string secretStringMock = "{\"username\":\"anyUser\",\"password\":\"anyPassword\",\"engine\":\"sqlserver\",\"host\":\"anyHost\",\"port\":\"1\",\"dbname\":\"anyName\"}";

    private string secretStringInvalidEngineMock = "{\"username\":\"anyUser\",\"password\":\"anyPassword\",\"engine\":\"invalid\",\"host\":\"anyHost\",\"port\":\"1\",\"dbname\":\"anyName\"}";

    /// <summary>
    /// Test the database config returns false when receive a empty secret.
    /// </summary>
    [Fact]
    public void TestDatabaseConfigWithEmptySecretReturnsFalse()
    {
        ISecretManager secretManager = Substitute.For<ISecretManager>();
        secretManager.Get().Returns(string.Empty);

        DatabaseConfig databaseConfig = new DatabaseConfig(secretManager);
        bool result = databaseConfig.StartConnection();
        Assert.False(result);
    }

    /// <summary>
    /// Test the database config returns false when receive a secret with an invalid engine.
    /// </summary>
    [Fact]
    public void TestDatabaseConfigWithInvalidEngineSecretReturnsFalse()
    {
        ISecretManager secretManager = Substitute.For<ISecretManager>();
        secretManager.Get().Returns(this.secretStringInvalidEngineMock);

        DatabaseConfig databaseConfig = new DatabaseConfig(secretManager);
        bool result = databaseConfig.StartConnection();
        Assert.False(result);
    }

    /// <summary>
    /// Test the database config returns false on exception.
    /// </summary>
    [Fact]
    public void TestDatabaseConfigReturnsFalseOnThrow()
    {
        ISecretManager secretManager = Substitute.For<ISecretManager>();
        secretManager.Get().Returns(x => throw new Exception());

        DatabaseConfig databaseConfig = new DatabaseConfig(secretManager);
        bool result = databaseConfig.StartConnection();
        Assert.False(result);
    }

    /// <summary>
    /// Test the database config connect with database success.
    /// </summary>
    [Fact]
    public void TestDatabaseConfigConnectWithDatabaseSuccess()
    {
        ISecretManager secretManager = Substitute.For<ISecretManager>();
        secretManager.Get().Returns(this.secretStringMock);

        DatabaseConfig databaseConfig = new DatabaseConfig(secretManager);
        bool result = databaseConfig.StartConnection();
        Assert.True(result);
    }
}
