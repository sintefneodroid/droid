using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc />
  /// <summary>
  /// Configurable for scaling
  /// </summary>
  [AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
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

    [SerializeField] SampleSpace3 _space = new SampleSpace3 {Space = Space3.ZeroOne};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._z);
    }

    public ISamplable ConfigurableValueSpace { get { return this._space; } }

    public override void UpdateCurrentConfiguration() { }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      //TODO: Denormalize configuration if space is marked as normalised

      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(debugging : this.Debugging,
                                 configuration : configuration,
                                 identifier : this.Identifier);
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
      var v = this._space.Sample();

      return new[] {
                       new Configuration(configurable_name : this._x, configurable_value : v.x),
                       new Configuration(configurable_name : this._y, configurable_value : v.y),
                       new Configuration(configurable_name : this._z, configurable_value : v.z)
                   };
    }
  }
}
