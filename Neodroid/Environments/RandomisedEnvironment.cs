using UnityEngine;
using Random = System.Random;

namespace Neodroid.Environments {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu("Neodroid/Environments/RandomisedEnvironment")]
  public class RandomisedEnvironment : PrototypingEnvironment {
    /// <summary>
    ///
    /// </summary>
    Random _random_generator = new Random();

    /// <summary>
    ///
    /// </summary>
    void RandomiseEnvironment() {
      foreach (var configurable in this.Configurables) {
        var value = configurable.Value.SampleConfiguration(this._random_generator);
        configurable.Value.ApplyConfiguration(value);
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
        this.Reset();

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
