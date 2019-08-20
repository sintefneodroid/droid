using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;
using NeodroidUtilities = droid.Runtime.Utilities.Extensions.NeodroidUtilities;

namespace droid.Runtime.Prototyping.Configurables {
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
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
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._z);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._rx);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._ry);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._rz);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          (Configurable)this,
                                                          this._rw);
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
      this.ParentEnvironment.UnRegister(this, this._rx);
      this.ParentEnvironment.UnRegister(this, this._ry);
      this.ParentEnvironment.UnRegister(this, this._rz);
      this.ParentEnvironment.UnRegister(this, this._rw);
    }

    public override ISpace ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        DebugPrinting.ApplyPrint(this.Debugging, configuration, this.Identifier);
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
      var x = Space1.ZeroOne.Sample();
      var y = Space1.ZeroOne.Sample();

      var a = new Vector2(x, y);
      var bounded = Vector2.Min(Vector2.Max(a, new Vector2(0.2f, 0.2f)), new Vector2(0.8f, 0.8f));

      //var z = Space1.ZeroOne.Sample() * this._camera.farClipPlane;
      var z = this._camera.nearClipPlane + 2;
      var bounded3 = new Vector3(bounded.x, bounded.y, z);

      var c = this._camera.ViewportToWorldPoint(bounded3);

      var b = new Quaternion(Space1.ZeroOne.Sample(),
                             Space1.ZeroOne.Sample(),
                             Space1.ZeroOne.Sample(),
                             Space1.ZeroOne.Sample());
      var sample1 = Space1.ZeroOne.Sample();
      var sample = Space1.ZeroOne.Sample();

      if (sample1 > 0.5f) {
        if (sample < .33f) {
          return new[] {new Configuration(this._x, c.x)};
        }

        if (sample > .66f) {
          return new[] {new Configuration(this._y, c.y)};
        }

        return new[] {new Configuration(this._z, c.z)};
      }

      if (sample < .33f) {
        return new[] {new Configuration(this._rx, b.x)};
      }

      if (sample > .66f) {
        return new[] {new Configuration(this._ry, b.y)};
      }

      return new[] {new Configuration(this._rz, b.z)};
    }
  }
}
