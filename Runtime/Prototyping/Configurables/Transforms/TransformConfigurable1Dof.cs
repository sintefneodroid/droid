using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Transforms {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "TransformConfigurable1Dof"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class TransformConfigurable1Dof : SpatialConfigurable,
                                           IHasSingle {
    #region Fields

    [SerializeField] Axis _axis_of_configuration = Axis.X_;
    [SerializeField] float _observation_value = 0;
    [SerializeField] SampleSpace1 _single_value_space = new SampleSpace1 {_space = Space1.ZeroOne};
    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    #endregion

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Transform" + this._axis_of_configuration + "Configurable"; }
    }

    /// <summary>
    ///
    /// </summary>
    public float ObservationValue {
      get { return this._observation_value; }
      private set { this._observation_value = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space1 SingleSpace {
      get { return (Space1)this._single_value_space.Space; }
      set { this._single_value_space.Space = value; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void UpdateCurrentConfiguration() {
      var transform1 = this.transform;
      Vector3 pos;
      if (this.coordinate_space == CoordinateSpace.Local_) {
        pos = transform1.localPosition;
      }else{
        pos = transform1.position;
        }
      var dir = transform1.forward;
      var rot = transform1.up;

      if (this.coordinate_space == CoordinateSpace.Environment_) {
        if (this.ParentEnvironment != null) {
          pos = this.ParentEnvironment.TransformPoint(pos);
          dir = this.ParentEnvironment.TransformDirection(dir);
          rot = this.ParentEnvironment.TransformDirection(rot);
        } else {
          Debug.LogWarning("ParentEnvironment not found!");
        }
      }

      switch (this._axis_of_configuration) {
        case Axis.X_:
          this._observation_value = pos.x;
          break;
        case Axis.Y_:
          this._observation_value = pos.y;
          break;
        case Axis.Z_:
          this._observation_value = pos.z;
          break;
        case Axis.Dir_x_:
          this._observation_value = dir.x;
          break;
        case Axis.Dir_y_:
          this._observation_value = dir.y;
          break;
        case Axis.Dir_z_:
          this._observation_value = dir.z;
          break;
        case Axis.Rot_x_:
          this._observation_value = rot.x;
          break;
        case Axis.Rot_y_:
          this._observation_value = rot.y;
          break;
        case Axis.Rot_z_:
          this._observation_value = rot.z;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        var dec_gran = 4;
        if (this._single_value_space.Space != null && this.ParentEnvironment.PlayableArea) {
          dec_gran = this._single_value_space.Space.DecimalGranularity;
        }

        if (this.ParentEnvironment) {
          switch (this._axis_of_configuration) {
            case Axis.X_:
              this.SingleSpace =
                  Space1.FromCenterExtent(this.ParentEnvironment.PlayableArea.Bounds.extents.x,
                                           decimal_granularity : dec_gran);
              break;
            case Axis.Y_:
              this.SingleSpace =
                  Space1.FromCenterExtent(this.ParentEnvironment.PlayableArea.Bounds.extents.y,
                                           decimal_granularity : dec_gran);
              break;
            case Axis.Z_:
              this.SingleSpace =
                  Space1.FromCenterExtent(this.ParentEnvironment.PlayableArea.Bounds.extents.z,
                                           decimal_granularity : dec_gran);
              break;
          }
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace { get { return this._single_value_space; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      float cv= this.SingleSpace.Reproject(simulator_configuration.ConfigurableValue);

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + simulator_configuration + " To " + this.Identifier);
      }
      #endif

      var transform1 = this.transform;
      Vector3 pos;
      if (this.coordinate_space == CoordinateSpace.Local_) {
        pos = transform1.localPosition;
      }else{
        pos = transform1.position;
      }
      var dir = transform1.forward;
      var rot = transform1.up;
      if (this.coordinate_space == CoordinateSpace.Environment_) {
        if (this.ParentEnvironment != null) {
          this.ParentEnvironment.TransformPoint(ref pos);
          this.ParentEnvironment.TransformDirection(ref dir);
          this.ParentEnvironment.TransformDirection(ref rot);
        } else {
          Debug.LogWarning("ParentEnvironment not found!");
        }
      }

      switch (this._axis_of_configuration) {
        case Axis.X_:
          if (this.RelativeToExistingValue) {
            pos.Set(cv + pos.x, pos.y, pos.z);
          } else {
            pos.Set(cv, pos.y, pos.z);
          }

          break;
        case Axis.Y_:
          if (this.RelativeToExistingValue) {
            pos.Set(pos.x, cv + pos.y, pos.z);
          } else {
            pos.Set(pos.x, cv, pos.z);
          }

          break;
        case Axis.Z_:
          if (this.RelativeToExistingValue) {
            pos.Set(pos.x, pos.y, cv + pos.z);
          } else {
            pos.Set(pos.x, pos.y, cv);
          }

          break;
        case Axis.Dir_x_:
          if (this.RelativeToExistingValue) {
            dir.Set(cv + dir.x, dir.y, dir.z);
          } else {
            dir.Set(cv, dir.y, dir.z);
          }

          break;
        case Axis.Dir_y_:
          if (this.RelativeToExistingValue) {
            dir.Set(dir.x, cv + dir.y, dir.z);
          } else {
            dir.Set(dir.x, cv, dir.z);
          }

          break;
        case Axis.Dir_z_:
          if (this.RelativeToExistingValue) {
            dir.Set(dir.x, dir.y, cv + dir.z);
          } else {
            dir.Set(dir.x, dir.y, cv);
          }

          break;
        case Axis.Rot_x_:
          if (this.RelativeToExistingValue) {
            rot.Set(cv + rot.x, rot.y, rot.z);
          } else {
            rot.Set(cv, rot.y, rot.z);
          }

          break;
        case Axis.Rot_y_:
          if (this.RelativeToExistingValue) {
            rot.Set(rot.x, cv + rot.y, rot.z);
          } else {
            rot.Set(rot.x, cv, rot.z);
          }

          break;
        case Axis.Rot_z_:
          if (this.RelativeToExistingValue) {
            rot.Set(rot.x, rot.y, cv + rot.z);
          } else {
            rot.Set(rot.x, rot.y, cv);
          }

          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      var inv_pos = pos;
      var inv_dir = dir;
      var inv_rot = rot;


      if (this.coordinate_space == CoordinateSpace.Environment_) {
        if (this.ParentEnvironment != null) {
          this.ParentEnvironment.InverseTransformPoint(ref inv_pos);
          this.ParentEnvironment.InverseTransformDirection(ref inv_dir);
          this.ParentEnvironment.InverseTransformDirection(ref inv_rot);
        } else {
          Debug.LogWarning("ParentEnvironment not found!");
        }
      }


      if (this.coordinate_space == CoordinateSpace.Local_) {
        transform1.localPosition = inv_pos;
      }else{
        this.transform.position = inv_pos;
      }


      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(inv_dir, inv_rot);
    }
  }
}
