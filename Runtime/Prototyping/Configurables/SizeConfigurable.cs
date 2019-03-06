using droid.Runtime.Environments;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Debugging;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using Random = System.Random;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath + "Size" + ConfigurableComponentMenuPath._Postfix)]
  public class SizeConfigurable : Configurable {
    /// <summary>
    ///   Alpha
    /// </summary>
    string _x;

    /// <summary>
    ///   Blue
    /// </summary>
    string _y;

    /// <summary>
    ///   Green
    /// </summary>
    string _z;

    [SerializeField] ValueSpace _space = new ValueSpace(){_Min_Value = 0.3f, _Max_Value = 1f};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._x);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._y);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, this._x);
      this.ParentEnvironment.UnRegister(this, this._y);
      this.ParentEnvironment.UnRegister(this, this._z);
    }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
      }
      #endif
      var local_scale = this.transform.localScale;
      if (configuration.ConfigurableName == this._x) {
        local_scale.x = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._y) {
        local_scale.y = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._z) {
        local_scale.z = configuration.ConfigurableValue;
      }

      this.transform.localScale = local_scale;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      var sample = random_generator.NextDouble();

      var v = this._space.Sample();

      if (sample < .33f) {
        return new Configuration(this._x, v);
      }

      if (sample > .66f) {
        return new Configuration(this._y, v);
      }

      return new Configuration(this._z, v);
    }
  }
}
