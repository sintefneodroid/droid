using Neodroid.Runtime.Interfaces;

namespace Neodroid.Runtime.Messaging.Messages {
  /// <summary>
  ///
  /// </summary>
  public class Configuration : IConfigurableConfiguration {
    public Configuration(string configurable_name, float configurable_value) {
      this.ConfigurableName = configurable_name;
      this.ConfigurableValue = configurable_value;
    }

    /// <summary>
    ///
    /// </summary>
    public string ConfigurableName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public float ConfigurableValue { get; set; }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<Configuration> " + this.ConfigurableName + ", " + this.ConfigurableValue + " </Configuration>";
    }
  }
}
