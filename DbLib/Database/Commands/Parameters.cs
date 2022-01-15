using System.Data;

namespace DbLib.Database;

/// <summary>
/// Database parameters.
/// </summary>
public class Parameters
{
    /// <summary>
    /// Gets or sets parameter name.
    /// </summary>
    /// <value>Parameter name.</value>
    public string ParameterName { get; set; }

    /// <summary>
    /// Gets or sets parameter value.
    /// </summary>
    /// <value>Parameter value.</value>
    public object Value { get; set; }

    /// <summary>
    /// Gets or sets parameter type.
    /// </summary>
    /// <value>Parameter type.</value>
    public DbType DbType { get; set; }

    /// <summary>
    /// Gets or sets parameter size.
    /// </summary>
    /// <value>Parameter size.</value>
    public int? Size { get; set; }

    /// <summary>
    /// Gets or sets the parameter direction like input parameters to procedures or functions.
    /// </summary>
    /// <value>Parameter direction.</value>
    public ParameterDirection ParamDirection { get; set; }
}