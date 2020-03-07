using UnityEngine;

namespace droid.Runtime.Environments.Prototyping {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/RandomisedEnvironment")]
  public class RandomisedEnvironment : PrototypingEnvironment {
    /// <summary>
    /// </summary>
    void RandomiseEnvironment() {
      foreach (var configurable in this.Configurables) {
        var value = configurable.Value.SampleConfigurations();
        foreach (var v in value) {
          configurable.Value.ApplyConfiguration(configuration : v);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      base.PreSetup();
      this.RandomiseEnvironment();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      if (this.Terminated) {
        this.Terminated = false;
        this.PrototypingReset();

        this.RandomiseEnvironment();
      }

      if (this.ShouldConfigure) {
        this.ShouldConfigure = false;
        this.Reconfigure();
      }

      this.LoopConfigurables();
      this.LoopSensors();
    }
  }
}
