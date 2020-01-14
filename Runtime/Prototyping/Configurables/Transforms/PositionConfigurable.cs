using System;
using droid.Runtime.Enums;
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
    [SerializeField] bool fetch_env_bounds = true;
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

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Space3 TripleSpace { get { return this._pos_space._space; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void RemotePostSetup() {
      if (this.fetch_env_bounds) {
        if (this.ParentEnvironment?.PlayableArea) {
          var dec_gran = 4;
          if (this._pos_space.Space != null) {
            dec_gran = this._pos_space.Space.DecimalGranularity;
          }

          this._pos_space.Space = Space3.FromCenterExtents(bounds_extents : this.ParentEnvironment.PlayableArea.Bounds.extents,
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
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, this, identifier : this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, this, identifier : this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, this, identifier : this._z);
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(this, identifier : this._x);
      this.ParentEnvironment.UnRegister(this, identifier : this._y);
      this.ParentEnvironment.UnRegister(this, identifier : this._z);
    }

    /// <summary>
    ///
    /// </summary>
    public ISamplable ConfigurableValueSpace { get { return this._pos_space; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateCurrentConfiguration() {
      switch(this._coordinate_spaceEnum){
        case CoordinateSpaceEnum.Environment_:         this._position = this.ParentEnvironment.TransformPoint(point : this.transform
        .position);
          break;
        case CoordinateSpaceEnum.Global_:         this._position = this.transform.position;
          break;
        case CoordinateSpaceEnum.Local_:         this._position = this.transform.localPosition;
          break;
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      Vector3 pos;
      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Local_) {
        pos = this.transform.localPosition;
      } else {
        pos = this.transform.position;
      }

      if (this._coordinate_spaceEnum==CoordinateSpaceEnum.Environment_) {
        pos = this.ParentEnvironment.TransformPoint(point : this.transform.position);
      }

      float v;

      if (simulator_configuration.ConfigurableName == this._x) {
        v = this._pos_space._space.Xspace.Reproject(v : simulator_configuration.ConfigurableValue);
      } else if (simulator_configuration.ConfigurableName == this._y) {
        v = this._pos_space._space.Yspace.Reproject(v : simulator_configuration.ConfigurableValue);
      } else {
        v = this._pos_space._space.Zspace.Reproject(v : simulator_configuration.ConfigurableValue);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(v + pos.x, newY : pos.y, newZ : pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(newX : pos.x, v + pos.y, newZ : pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(newX : pos.x, newY : pos.y, v + pos.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          pos.Set(newX : v, newY : pos.y, newZ : pos.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          pos.Set(newX : pos.x, newY : v, newZ : pos.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          pos.Set(newX : pos.x, newY : pos.y, newZ : v);
        }
      }

      var inv_pos = pos;
      if (this._coordinate_spaceEnum==CoordinateSpaceEnum.Environment_) {
        inv_pos = this.ParentEnvironment.InverseTransformPoint(point : inv_pos);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Setting pos of {this} to {inv_pos}, from {pos} and r {simulator_configuration.ConfigurableValue}");
      }
      #endif

      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Local_) {
         this.transform.localPosition=inv_pos;
      } else {
        this.transform.position = inv_pos;
      }


    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this._pos_space.Sample();
      return new[] {
                       new Configuration(configurable_name : this._x, configurable_value : sample.x),
                       new Configuration(configurable_name : this._y, configurable_value : sample.y),
                       new Configuration(configurable_name : this._z, configurable_value : sample.z)
                   };
    }
  }
}
