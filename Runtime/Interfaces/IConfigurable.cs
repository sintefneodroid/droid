using System;

namespace droid.Runtime.Interfaces {
  public interface IConfigurable : IRegisterable {
    void UpdateCurrentConfiguration();
    void ApplyConfiguration(IConfigurableConfiguration configuration);
    void EnvironmentReset();
    IConfigurableConfiguration[] SampleConfigurations();

    bool SampleRandom { get; set; }

    void PostEnvironmentSetup();
    void Tick();
  }
}
