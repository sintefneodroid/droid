using System;
using droid.Runtime.Interfaces;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class Configuration : IConfigurableConfiguration {
    public Configuration(string configurable_name, float configurable_value, bool sample_random = false) {
      this.ConfigurableName = configurable_name;
      this.ConfigurableValue = configurable_value;
      this.SampleRandom = sample_random;
    }

    /// <summary>
    /// </summary>
    public string ConfigurableName { get; set; }

    /// <summary>
    ///
    /// </summary>
    public Boolean SampleRandom { get; set; }

    /// <summary>
    /// </summary>
    public float ConfigurableValue { get; set; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<Configuration> " + this.ConfigurableName + ", " + this.ConfigurableValue + " </Configuration>";
    }
  }
}
