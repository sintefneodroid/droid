using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "Position"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class PositionConfigurable : SpatialConfigurable,
                                      IHasTriple {
    #region Fields

    [SerializeField] Vector3 _position = Vector3.zero;
    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;
    [SerializeField] bool _use_environments_space = false;
    [SerializeField] SampleSpace3 _pos_space = new SampleSpace3 {Space = Space3.ZeroOne};

    #endregion

    /// <summary>
    /// </summary>
    string _x;

    /// <summary>
    /// </summary>
    string _y;

    /// <summary>
    /// </summary>
    string _z;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 ObservationValue { get { return this._position; } }

    /// <summary>
    ///
    /// </summary>
    public Space3 TripleSpace { get { return this._pos_space._space; } }

    /// <summary>
    ///
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment?.PlayableArea) {
          var dec_gran = 4;
          if (this._pos_space.Space != null) {
            dec_gran = this._pos_space.Space.DecimalGranularity;
          }

          this._pos_space.Space = Space3.FromCenterExtents(this.ParentEnvironment.PlayableArea.Bounds.extents,
                                                       decimal_granularity : dec_gran);
        }
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("PreSetup");
      }
      #endif

      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._z);
    }

    /// <summary>
    ///
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
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace { get { return this._pos_space; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space) {
        this._position = this.ParentEnvironment.TransformPoint(this.transform.position);
      } else {
        this._position = this.transform.position;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      var pos = this.transform.position;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPoint(this.transform.position);
      }

      float v;
      if (this._pos_space._space.normalised) {
        if (simulator_configuration.ConfigurableName == this._x) {
          v = this._pos_space._space.Xspace.ClipRoundDenormaliseClip(simulator_configuration.ConfigurableValue);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          v = this._pos_space._space.Yspace.ClipRoundDenormaliseClip(simulator_configuration.ConfigurableValue);
        } else {
          v = this._pos_space._space.Zspace.ClipRoundDenormaliseClip(simulator_configuration.ConfigurableValue);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          v = this._pos_space._space.Xspace.ClipRound(simulator_configuration.ConfigurableValue);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          v = this._pos_space._space.Yspace.ClipRound(simulator_configuration.ConfigurableValue);
        } else {
          v = this._pos_space._space.Zspace.ClipRound(simulator_configuration.ConfigurableValue);
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(v + pos.x, pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v + pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v + pos.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(v, pos.y, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v, pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v);
        }
      }

      var inv_pos = pos;
      if (this._use_environments_space) {
        inv_pos = this.ParentEnvironment.InverseTransformPoint(inv_pos);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Setting pos of {this} to {inv_pos}, from {pos} and r {simulator_configuration.ConfigurableValue}");
      }
      #endif

      this.transform.position = inv_pos;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this._pos_space.Sample();
      return new[] {
                       new Configuration(this._x, sample.x),
                       new Configuration(this._y, sample.y),
                       new Configuration(this._z, sample.z)
                   };
    }
  }
}
