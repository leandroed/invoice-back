using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Serilog;

namespace InvoiceApi.Vendor;

public class AwsSecretManager : ISecretManager
{
    /// <summary>
    /// Gets or sets the secret name.
    /// </summary>
    /// <value>Secret name.</value>
    public string SecretName
    {
        get
        {
            string secret = string.Empty;

            try
            {
                secret = Environment.GetEnvironmentVariable("DATABASE_SECRET");
            }
            catch (Exception ex)
            {
                Log.Error($"Error the database secret enviroment variable was not found. {ex.Message}");
                return string.Empty;
            }

            return secret;
        }
    }

    /// <summary>
    /// Gets the json secret in aws secret manager.
    /// </summary>
    /// <returns>Json string response.</returns>
    public string Get()
    {
        string secret = string.Empty;
        string secretName = this.SecretName;
        GetSecretValueResponse response = null;
        AmazonSecretsManagerConfig config = new AmazonSecretsManagerConfig { RegionEndpoint = RegionEndpoint.USEast2 };
        AmazonSecretsManagerClient client = new AmazonSecretsManagerClient(config);

        if (string.IsNullOrEmpty(secretName))
        {
            return secret;
        }

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
        };

        try
        {
            response = client.GetSecretValueAsync(request).Result;
            secret = response?.SecretString;
        }
        catch (Exception ex)
        {
            Log.Error($"Error the requested secret '{secretName}' was not found. {ex.Message}");
        }

        return secret;
    }
}
