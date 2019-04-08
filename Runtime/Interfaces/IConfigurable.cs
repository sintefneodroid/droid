using System;
using droid.Runtime.Messaging.Messages;

namespace droid.Runtime.Interfaces {
  public interface IConfigurable : IRegisterable {
    void UpdateCurrentConfiguration();
    void ApplyConfiguration(IConfigurableConfiguration configuration);
    void EnvironmentReset();
    Configuration[] SampleConfigurations();

    bool SampleRandom { get; set; }

    void PostEnvironmentSetup();
    void Tick();
  }
}
