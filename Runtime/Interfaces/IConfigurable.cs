using droid.Runtime.Enums;
using droid.Runtime.Messaging.Messages;

namespace droid.Runtime.Interfaces {
  /// <summary>
  ///
  /// </summary>
  public interface IConfigurable : IRegisterable {
    /// <summary>
    ///
    /// </summary>
    void UpdateCurrentConfiguration();

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    void ApplyConfiguration(IConfigurableConfiguration configuration);


    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    Configuration[] SampleConfigurations();

    /// <summary>
    ///
    /// </summary>
    RandomSamplingMode RandomSamplingMode { get; set; }

  }
}
