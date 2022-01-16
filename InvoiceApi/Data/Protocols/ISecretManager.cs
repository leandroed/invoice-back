namespace InvoiceApi.Protocols;

/// <summary>
/// Secret manager interface.
/// </summary>
public interface ISecretManager
{
    string SecretName { get; }

    /// <summary>
    /// Gets the json secret in aws secret manager.
    /// </summary>
    /// <returns>Json string response.</returns>
    string Get();
}
