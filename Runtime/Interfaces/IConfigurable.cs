using System;

namespace droid.Runtime.Interfaces {
  public interface IConfigurable : IRegisterable {
    void UpdateCurrentConfiguration();
    void ApplyConfiguration(IConfigurableConfiguration configuration);
    void EnvironmentReset();
    IConfigurableConfiguration SampleConfiguration(Random random_generator);
  }
}
