using UnityEngine;

namespace droid.Runtime.Environments {
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
        foreach (var v in value)
        {
          configurable.Value.ApplyConfiguration(v);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      base.PreSetup();
      this.RandomiseEnvironment();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PostStep() {
      if (this._Terminated) {
        this._Terminated = false;
        this.EnvironmentReset();

        this.RandomiseEnvironment();
      }

      if (this._Configure) {
        this._Configure = false;
        this.Configure();
      }

      this.UpdateConfigurableValues();
      this.UpdateObserversData();
    }
  }
}
