using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.GameObjects.BoundingBoxes;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "TransformConfigurable1Dof"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class TransformConfigurable1Dof : Configurable,
                                           IHasSingle {
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Transform" + this._axis_of_configuration + "Configurable"; }
    }

    public float ObservationValue {
      get { return this._observation_value; }
      private set { this._observation_value = value; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space1 SingleSpace {
      get { return this._single_value_space; }
      set { this._single_value_space = value; }
    }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void UpdateCurrentConfiguration() {
      var transform1 = this.transform;
      var pos = transform1.position;
      var dir = transform1.forward;
      var rot = transform1.up;
      if (this._use_environments_space) {
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
    protected override void PreSetup() {
      if (this._use_bounding_box_for_range) {
        if (this._bounding_box != null) {
          var valid_input = new Space1 {
                                           _Max_Value =
                                               Math.Min(this._bounding_box.Bounds.size.x,
                                                        Math.Min(this._bounding_box.Bounds.size.y,
                                                                 this._bounding_box.Bounds.size.z))
                                       };
          valid_input._Min_Value = -valid_input._Max_Value;
          this.SingleSpace = valid_input;
        }
      }
    }

    public override ISpace ConfigurableValueSpace { get { return this.SingleSpace; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      if (simulator_configuration.ConfigurableValue < this.SingleSpace._Min_Value
          || simulator_configuration.ConfigurableValue > this.SingleSpace._Max_Value) {
        Debug.Log($"It does not accept input, outside allowed range {this.SingleSpace._Min_Value} to {this.SingleSpace._Max_Value}");
        return; // Do nothing
      }

      var cv = this.SingleSpace.Round(simulator_configuration.ConfigurableValue);

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + simulator_configuration + " To " + this.Identifier);
      }
      #endif

      var transform1 = this.transform;
      var pos = transform1.position;
      var dir = transform1.forward;
      var rot = transform1.up;
      if (this._use_environments_space) {
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
            pos.Set(cv - pos.x, pos.y, pos.z);
          } else {
            pos.Set(cv, pos.y, pos.z);
          }

          break;
        case Axis.Y_:
          if (this.RelativeToExistingValue) {
            pos.Set(pos.x, cv - pos.y, pos.z);
          } else {
            pos.Set(pos.x, cv, pos.z);
          }

          break;
        case Axis.Z_:
          if (this.RelativeToExistingValue) {
            pos.Set(pos.x, pos.y, cv - pos.z);
          } else {
            pos.Set(pos.x, pos.y, cv);
          }

          break;
        case Axis.Dir_x_:
          if (this.RelativeToExistingValue) {
            dir.Set(cv - dir.x, dir.y, dir.z);
          } else {
            dir.Set(cv, dir.y, dir.z);
          }

          break;
        case Axis.Dir_y_:
          if (this.RelativeToExistingValue) {
            dir.Set(dir.x, cv - dir.y, dir.z);
          } else {
            dir.Set(dir.x, cv, dir.z);
          }

          break;
        case Axis.Dir_z_:
          if (this.RelativeToExistingValue) {
            dir.Set(dir.x, dir.y, cv - dir.z);
          } else {
            dir.Set(dir.x, dir.y, cv);
          }

          break;
        case Axis.Rot_x_:
          if (this.RelativeToExistingValue) {
            rot.Set(cv - rot.x, rot.y, rot.z);
          } else {
            rot.Set(cv, rot.y, rot.z);
          }

          break;
        case Axis.Rot_y_:
          if (this.RelativeToExistingValue) {
            rot.Set(rot.x, cv - rot.y, rot.z);
          } else {
            rot.Set(rot.x, cv, rot.z);
          }

          break;
        case Axis.Rot_z_:
          if (this.RelativeToExistingValue) {
            rot.Set(rot.x, rot.y, cv - rot.z);
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
      if (this._use_environments_space) {
        if (this.ParentEnvironment != null) {
          this.ParentEnvironment.InverseTransformPoint(ref inv_pos);
          this.ParentEnvironment.InverseTransformDirection(ref inv_dir);
          this.ParentEnvironment.InverseTransformDirection(ref inv_rot);
        } else {
          Debug.LogWarning("ParentEnvironment not found!");
        }
      }

      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(inv_dir, inv_rot);
    }

    #region Fields

    [SerializeField] Axis _axis_of_configuration = Axis.X_;
    [SerializeField] BoundingBox _bounding_box = null;
    [SerializeField] bool _use_bounding_box_for_range = false;
    [SerializeField] float _observation_value = 0;
    [SerializeField] bool _use_environments_space = false;
    [SerializeField] Space1 _single_value_space = Space1.ZeroOne;

    #endregion
  }
}
