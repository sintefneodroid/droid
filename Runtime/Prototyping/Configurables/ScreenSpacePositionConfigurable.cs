using System.Collections.Generic;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Misc;
using UnityEngine;
using Random = System.Random;

namespace Neodroid.Runtime.Prototyping.Configurables {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "ScreenSpacePosition"
      + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Renderer))]
  public class ScreenSpacePositionConfigurable : Configurable {
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

    /// <summary>
    ///   Red
    /// </summary>
    string _rx;

    string _ry;
    string _rw;
    string _rz;

    /// <summary>
    /// </summary>
    [SerializeField] Camera _camera;

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

      if (!this._camera) this._camera = FindObjectOfType<Camera>();
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
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._rx);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._ry);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._rz);
      this.ParentEnvironment = NeodroidUtilities.RegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
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

    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + configuration + " To " + this.Identifier);
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
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(Random random_generator) {
      var x = random_generator.NextDouble();
      var y = random_generator.NextDouble();

      var a = new Vector2((float)x, (float)y);
      var bounded = Vector2.Min(Vector2.Max(a, new Vector2(0.2f, 0.2f)), new Vector2(0.8f, 0.8f));

      //var z = random_generator.NextDouble() * this._camera.farClipPlane;
      var z = this._camera.nearClipPlane + 2;
      var bounded3 = new Vector3(bounded.x, bounded.y, z);

      var c = this._camera.ViewportToWorldPoint(bounded3);

      var b = new Quaternion(
          (float)random_generator.NextDouble(),
          (float)random_generator.NextDouble(),
          (float)random_generator.NextDouble(),
          (float)random_generator.NextDouble());
      var sample1 = random_generator.NextDouble();
      var sample = random_generator.NextDouble();

      if (sample1 > 0.5f) {
        if (sample < .33f) {
          return new Configuration(this._x, c.x);
        }

        if (sample > .66f) {
          return new Configuration(this._y, c.y);
        }

        return new Configuration(this._z, c.z);
      } else {
        if (sample < .33f) {
          return new Configuration(this._rx, b.x);
        }

        if (sample > .66f) {
          return new Configuration(this._ry, b.y);
        }

        return new Configuration(this._rz, b.z);
      }
    }
  }
}
