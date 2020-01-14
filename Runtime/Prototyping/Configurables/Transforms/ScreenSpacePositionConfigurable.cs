using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "ScreenSpacePosition"
                    + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class ScreenSpacePositionConfigurable : Configurable {
    string _x;
    string _y;
    string _z;
    string _rx;
    string _ry;
    string _rw;
    string _rz;

    /// <summary>
    /// </summary>
    [SerializeField]
    Camera _camera;

    [SerializeField] SampleSpace3 _configurable_value_space;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
      this._rx = this.Identifier + "RX";
      this._ry = this.Identifier + "RY";
      this._rz = this.Identifier + "RZ";
      this._rw = this.Identifier + "RW";

      if (!this._camera) {
        this._camera = FindObjectOfType<Camera>();
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._z);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._rx);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._ry);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._rz);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          (Configurable)this,
                                                          identifier : this._rw);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, identifier : this._x);
      this.ParentEnvironment.UnRegister(this, identifier : this._y);
      this.ParentEnvironment.UnRegister(this, identifier : this._z);
      this.ParentEnvironment.UnRegister(this, identifier : this._rx);
      this.ParentEnvironment.UnRegister(this, identifier : this._ry);
      this.ParentEnvironment.UnRegister(this, identifier : this._rz);
      this.ParentEnvironment.UnRegister(this, identifier : this._rw);
    }

    public  ISamplable ConfigurableValueSpace { get { return this._configurable_value_space; } }

    public override void UpdateCurrentConfiguration() {  }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {

       var  cv = this._configurable_value_space.Space.Reproject(configuration_configurable_value : configuration.ConfigurableValue);



      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(debugging : this.Debugging, configuration : configuration, identifier : this.Identifier);
      }
      #endif

      var pos = this.transform.position;
      var rot = this.transform.rotation;

      if (configuration.ConfigurableName == this._x) {
        pos.x = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._y) {
        pos.y = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._z) {
        pos.z = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._rx) {
        rot.x = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._ry) {
        rot.y = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._rz) {
        rot.z = configuration.ConfigurableValue;
      } else if (configuration.ConfigurableName == this._rw) {
        rot.w = configuration.ConfigurableValue;
      }

      this.transform.position = pos;
      this.transform.rotation = rot;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var x = this.ConfigurableValueSpace.Sample();
      var y = this.ConfigurableValueSpace.Sample();

      var a = new Vector2(x : x, y : y);
      var bounded = Vector2.Min(Vector2.Max(lhs : a, new Vector2(0.2f, 0.2f)), new Vector2(0.8f, 0.8f));

      //var z = Space1.ZeroOne.Sample() * this._camera.farClipPlane;
      var z = this._camera.nearClipPlane + 2;
      var bounded3 = new Vector3(x : bounded.x, y : bounded.y, z : z);

      var c = this._camera.ViewportToWorldPoint(position : bounded3);

      var b = new Quaternion(this.ConfigurableValueSpace.Sample(),
                             this.ConfigurableValueSpace.Sample(),
                             this.ConfigurableValueSpace.Sample(),
                             this.ConfigurableValueSpace.Sample());
      var sample1 = this.ConfigurableValueSpace.Sample();
      var sample = this.ConfigurableValueSpace.Sample();

      if (sample1 > 0.5f) {
        if (sample < .33f) {
          return new[] {new Configuration(configurable_name : this._x, configurable_value : c.x)};
        }

        if (sample > .66f) {
          return new[] {new Configuration(configurable_name : this._y, configurable_value : c.y)};
        }

        return new[] {new Configuration(configurable_name : this._z, configurable_value : c.z)};
      }

      if (sample < .33f) {
        return new[] {new Configuration(configurable_name : this._rx, configurable_value : b.x)};
      }

      if (sample > .66f) {
        return new[] {new Configuration(configurable_name : this._ry, configurable_value : b.y)};
      }

      return new[] {new Configuration(configurable_name : this._rz, configurable_value : b.z)};
    }
  }
}
