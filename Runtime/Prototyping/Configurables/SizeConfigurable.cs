using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Size"
                    + ConfigurableComponentMenuPath._Postfix)]
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

    [SerializeField] Space3 _space = Space3.ZeroOne;

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
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._x);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._y);
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._z);
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
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = Space1.ZeroOne.Sample();

      var v = this._space.Sample();

      if (sample < .33f) {
        return new[] {new Configuration(this._x, v.x)};
      }

      if (sample > .66f) {
        return new[] {new Configuration(this._y, v.y)};
      }

      return new[] {new Configuration(this._z, v.z)};
    }
  }
}
