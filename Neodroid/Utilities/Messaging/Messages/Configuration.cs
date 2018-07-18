namespace Neodroid.Utilities.Messaging.Messages {
  /// <summary>
  ///
  /// </summary>
  public class Configuration {
    public Configuration(string configurable_name, float configurable_value) {
      this.ConfigurableName = configurable_name;
      this.ConfigurableValue = configurable_value;
    }

    /// <summary>
    ///
    /// </summary>
    public string ConfigurableName { get; }

    /// <summary>
    ///
    /// </summary>
    public float ConfigurableValue { get; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<Configuration> " + this.ConfigurableName + ", " + this.ConfigurableValue + " </Configuration>";
    }
  }
}
