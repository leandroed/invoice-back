public interface ISecretManager
{
    /// <summary>
    /// Gets the json secret in aws secret manager.
    /// </summary>
    /// <returns>Json string response.</returns>
    string Get();
}
