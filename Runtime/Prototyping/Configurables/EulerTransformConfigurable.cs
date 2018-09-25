using System;
using Neodroid.Runtime.Environments;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Messaging.Messages;
using Neodroid.Runtime.Utilities.Structs;
using Neodroid.Runtime.Utilities.Unsorted;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "EulerTransform"
      + ConfigurableComponentMenuPath._Postfix)]
  public class EulerTransformConfigurable : Configurable,
                                            IHasEulerTransform {
    string _dir_x;
    string _dir_y;
    string _dir_z;

    [SerializeField] Vector3 _direction;

    [Header("Observation", order = 103), SerializeField]
    Vector3 _position;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    [SerializeField] Vector3 _rotation;

    [SerializeField] bool _use_environments_space;

    string _x;
    string _y;
    string _z;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return "EulerTransformConfigurable"; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 Position {
      get { return this._position; }
      set { this._position = value; }
    }

    public Vector3 Direction {
      get { return this._direction; }
      set { this._direction = value; }
    }

    public Vector3 Rotation {
      get { return this._rotation; }
      set { this._rotation = value; }
    }

    public Space3 PositionSpace { get; } = new Space3();
    public Space3 DirectionSpace { get; } = new Space3();
    public Space3 RotationSpace { get; } = new Space3();

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space) {
        this.Position = this.ParentEnvironment.TransformPosition(this.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(this.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(this.transform.up);
      } else {
        this.Position = this.transform.position;
        this.Direction = this.transform.forward;
        this.Rotation = this.transform.up;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._z);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._dir_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._dir_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._dir_z);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._rot_x);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._rot_y);
      this.ParentEnvironment = NeodroidUtilities.MaybeRegisterNamedComponent(
          (PrototypingEnvironment)this.ParentEnvironment,
          (Configurable)this,
          this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      this._dir_x = this.Identifier + "DirX_";
      this._dir_y = this.Identifier + "DirY_";
      this._dir_z = this.Identifier + "DirZ_";
      this._rot_x = this.Identifier + "RotX_";
      this._rot_y = this.Identifier + "RotY_";
      this._rot_z = this.Identifier + "RotZ_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) return;
      this.ParentEnvironment.UnRegister(this, this._x);
      this.ParentEnvironment.UnRegister(this, this._y);
      this.ParentEnvironment.UnRegister(this, this._z);
      this.ParentEnvironment.UnRegister(this, this._dir_x);
      this.ParentEnvironment.UnRegister(this, this._dir_y);
      this.ParentEnvironment.UnRegister(this, this._dir_z);
      this.ParentEnvironment.UnRegister(this, this._rot_x);
      this.ParentEnvironment.UnRegister(this, this._rot_y);
      this.ParentEnvironment.UnRegister(this, this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration configuration) {
      var pos = this.transform.position;
      var dir = this.transform.forward;
      var rot = this.transform.up;
      if (this._use_environments_space) {
        pos = this.ParentEnvironment.TransformPosition(pos);
        dir = this.ParentEnvironment.TransformDirection(dir);
        rot = this.ParentEnvironment.TransformDirection(rot);
      }

      var v = configuration.ConfigurableValue;
      if (this.PositionSpace._Decimal_Granularity >= 0) {
        v = (int)Math.Round(v, this.PositionSpace._Decimal_Granularity);
      }

      if (this.PositionSpace._Min_Values[0].CompareTo(this.PositionSpace._Max_Values[0]) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, PositionSpace._Min_Values == PositionSpace._Max_Values, and use other space aswell!
        if (v < this.PositionSpace._Min_Values[0] || v > this.PositionSpace._Max_Values[0]) {
          Debug.Log(
              string.Format(
                  "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                  this.PositionSpace._Min_Values[0],
                  this.PositionSpace._Max_Values[0],
                  v));
          return; // Do nothing
        }
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log("Applying " + v + " To " + this.Identifier);
      }
      #endif
      if (this.RelativeToExistingValue) {
        if (configuration.ConfigurableName == this._x) {
          pos.Set(v - pos.x, pos.y, pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v - pos.y, pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v - pos.z);
        } else if (configuration.ConfigurableName == this._dir_x) {
          dir.Set(v - dir.x, dir.y, dir.z);
        } else if (configuration.ConfigurableName == this._dir_y) {
          dir.Set(dir.x, v - dir.y, dir.z);
        } else if (configuration.ConfigurableName == this._dir_z) {
          dir.Set(dir.x, dir.y, v - dir.z);
        } else if (configuration.ConfigurableName == this._rot_x) {
          rot.Set(v - rot.x, rot.y, rot.z);
        } else if (configuration.ConfigurableName == this._rot_y) {
          rot.Set(rot.x, v - rot.y, rot.z);
        } else if (configuration.ConfigurableName == this._rot_z) {
          rot.Set(rot.x, rot.y, v - rot.z);
        }
      } else {
        if (configuration.ConfigurableName == this._x) {
          pos.Set(v, pos.y, pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(pos.x, v, pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(pos.x, pos.y, v);
        } else if (configuration.ConfigurableName == this._dir_x) {
          dir.Set(v, dir.y, dir.z);
        } else if (configuration.ConfigurableName == this._dir_y) {
          dir.Set(dir.x, v, dir.z);
        } else if (configuration.ConfigurableName == this._dir_z) {
          dir.Set(dir.x, dir.y, v);
        } else if (configuration.ConfigurableName == this._rot_x) {
          rot.Set(v, rot.y, rot.z);
        } else if (configuration.ConfigurableName == this._rot_y) {
          rot.Set(rot.x, v, rot.z);
        } else if (configuration.ConfigurableName == this._rot_z) {
          rot.Set(rot.x, rot.y, v);
        }
      }

      var inv_pos = pos;
      var inv_dir = dir;
      var inv_rot = rot;
      if (this._use_environments_space) {
        inv_pos = this.ParentEnvironment.InverseTransformPosition(pos);
        inv_dir = this.ParentEnvironment.InverseTransformDirection(dir);
        inv_rot = this.ParentEnvironment.InverseTransformDirection(rot);
      }

      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(inv_dir, inv_rot);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    public override IConfigurableConfiguration SampleConfiguration(System.Random random_generator) {
      return new Configuration(this.Identifier, random_generator.Next());
    }
  }
}