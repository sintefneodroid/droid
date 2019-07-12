using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Rotation"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class RotationConfigurable : Configurable,
                                      IHasQuadruple {
    [SerializeField] Space4 _quad_space = Space4.MinusOneOne;
    [SerializeField] bool _use_environments_space = false;

    [Header("Observation", order = 103)]
    [SerializeField]
    Quaternion observation_value = Quaternion.identity;

    /// <summary>
    /// </summary>
    string _x;

    /// <summary>
    /// </summary>
    string _y;

    /// <summary>
    /// </summary>
    string _z;

    /// <summary>
    /// </summary>
    string _w;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Quaternion ObservationValue { get { return this.observation_value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space4 QuadSpace { get { return this._quad_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      this._w = this.Identifier + "W_";
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
      this.ParentEnvironment =
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this, this._w);
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
      this.ParentEnvironment.UnRegister(this, this._w);
    }

    public override ISpace ConfigurableValueSpace { get; }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space && this.ParentEnvironment != null) {
        this.observation_value = this.ParentEnvironment.TransformRotation(this.transform.rotation);
      } else {
        this.observation_value = this.transform.rotation;
      }
    }

    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      var rot = this.transform.rotation;
      if (this.ParentEnvironment && this._use_environments_space) {
        rot = this.ParentEnvironment.TransformRotation(this.transform.rotation);
      }

      var v = simulator_configuration.ConfigurableValue;
      if (this.QuadSpace.DecimalGranularity >= 0) {
        v = (int)Math.Round(v, this.QuadSpace.DecimalGranularity);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          if (this.QuadSpace.MinValues.x.CompareTo(this.QuadSpace.MaxValues.x) != 0) {
            if (v < this.QuadSpace.MinValues.x || v > this.QuadSpace.MaxValues.x) {
              Debug.Log($"ConfigurableX does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.x} to {this.QuadSpace.MaxValues.x}");
              return; // Do nothing
            }
          }

          rot.Set(rot.x - v, rot.y, rot.z, rot.w);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          if (this.QuadSpace.MinValues.y.CompareTo(this.QuadSpace.MaxValues.y) != 0) {
            if (v < this.QuadSpace.MinValues.y || v > this.QuadSpace.MaxValues.y) {
              Debug.Log($"ConfigurableY does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.y} to {this.QuadSpace.MaxValues.y}");
              return; // Do nothing
            }
          }

          rot.Set(rot.x, rot.y - v, rot.z, rot.w);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          if (this.QuadSpace.MinValues.z.CompareTo(this.QuadSpace.MaxValues.z) != 0) {
            if (v < this.QuadSpace.MinValues.z || v > this.QuadSpace.MaxValues.z) {
              Debug.Log($"ConfigurableZ does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.z} to {this.QuadSpace.MaxValues.z}");
              return; // Do nothing
            }
          }

          rot.Set(rot.x, rot.y, rot.z - v, rot.w);
        } else if (simulator_configuration.ConfigurableName == this._w) {
          if (this.QuadSpace.MinValues.w.CompareTo(this.QuadSpace.MaxValues.w) != 0) {
            if (v < this.QuadSpace.MinValues.w || v > this.QuadSpace.MaxValues.w) {
              Debug.Log($"ConfigurableW does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.w} to {this.QuadSpace.MaxValues.w}");
              return; // Do nothing
            }
          }

          rot.Set(rot.x, rot.y, rot.z, rot.w - v);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          if (this.QuadSpace.MinValues.x.CompareTo(this.QuadSpace.MaxValues.x) != 0) {
            if (v < this.QuadSpace.MinValues.x || v > this.QuadSpace.MaxValues.x) {
              Debug.Log($"ConfigurableX does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.x} to {this.QuadSpace.MaxValues.x}");
              return; // Do nothing
            }
          }

          rot.Set(v, rot.y, rot.z, rot.w);
          //rot.x = v;
        } else if (simulator_configuration.ConfigurableName == this._y) {
          if (this.QuadSpace.MinValues.y.CompareTo(this.QuadSpace.MaxValues.y) != 0) {
            if (v < this.QuadSpace.MinValues.y || v > this.QuadSpace.MaxValues.y) {
              Debug.Log($"ConfigurableY does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.y} to {this.QuadSpace.MaxValues.y}");
              return; // Do nothing
            }
          }

          rot.Set(rot.x, v, rot.z, rot.w);
          //rot.y = v;
        } else if (simulator_configuration.ConfigurableName == this._z) {
          if (this.QuadSpace.MinValues.z.CompareTo(this.QuadSpace.MaxValues.z) != 0) {
            if (v < this.QuadSpace.MinValues.z || v > this.QuadSpace.MaxValues.z) {
              Debug.Log($"ConfigurableZ does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.z} to {this.QuadSpace.MaxValues.z}");
              return; // Do nothing
            }
          }

          rot.Set(rot.x, rot.y, v, rot.w);
          //rot.z = v;
        } else if (simulator_configuration.ConfigurableName == this._w) {
          if (this.QuadSpace.MinValues.w.CompareTo(this.QuadSpace.MaxValues.w) != 0) {
            if (v < this.QuadSpace.MinValues.w || v > this.QuadSpace.MaxValues.w) {
              Debug.Log($"ConfigurableW does not accept input {v}, outside allowed range "
                        + $"{this.QuadSpace.MinValues.w} to {this.QuadSpace.MaxValues.w}");
              return; // Do nothing
            }
          }

          rot.Set(rot.x, rot.y, rot.z, v);
          //rot.w = v;
        }
      }

      if (this.ParentEnvironment && this._use_environments_space) {
        rot = this.ParentEnvironment.InverseTransformRotation(rot);
      }

      this.transform.rotation = rot;
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this.QuadSpace.Sample();

      var r = Random.Range(0, 4);
      switch (r) {
        case 0:
          return new[] {new Configuration(this._x, sample.x)};

        case 1:
          return new[] {new Configuration(this._y, sample.y)};

        case 2:
          return new[] {new Configuration(this._z, sample.z)};

        case 3:
          return new[] {new Configuration(this._w, sample.w)};

        default:
          throw new IndexOutOfRangeException();
      }
    }
  }
}
