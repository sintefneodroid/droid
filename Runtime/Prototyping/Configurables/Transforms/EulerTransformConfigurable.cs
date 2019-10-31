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
                    + "EulerTransform"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class EulerTransformConfigurable : SpatialConfigurable,
                                            IHasEulerTransform {
    string _dir_x;
    string _dir_y;
    string _dir_z;

    string _x;
    string _y;
    string _z;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Vector3 _direction;
    [SerializeField] Vector3 _rotation;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "EulerTransformConfigurable"; } }

    /// <summary>
    /// </summary>
    public Vector3 Position { get { return this._position; } set { this._position = value; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Direction { get { return this._direction; } set { this._direction = value; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Rotation { get { return this._rotation; } set { this._rotation = value; } }

    [SerializeField]SampleSpace3 pos_space = new SampleSpace3();
    [SerializeField]SampleSpace3 dir_space = new SampleSpace3();
    [SerializeField]SampleSpace3 rot_space = new SampleSpace3();

    /// <summary>
    ///
    /// </summary>
    public Space3 PositionSpace { get { return (Space3)this.pos_space.Space; } }

    /// <summary>
    ///
    /// </summary>
    public Space3 DirectionSpace { get { return (Space3)this.dir_space.Space; } }

    /// <summary>
    ///
    /// </summary>
    public Space3 RotationSpace { get { return (Space3)this.rot_space.Space; } }

    /// <summary>
    ///
    /// </summary>
    public override ISamplable ConfigurableValueSpace {
      get {
        return this.pos_space;
        //return DirectionSpace;
        //return RotationSpace;
      }
    }

    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() { //TODO: IMPLEMENT LOCAL SPACE
      if (this.coordinate_space == CoordinateSpace.Environment_) {
        this.Position = this.ParentEnvironment.TransformPoint(this.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(this.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(this.transform.up);
      } else {
        var transform1 = this.transform;
        this.Position = transform1.position;
        this.Direction = transform1.forward;
        this.Rotation = transform1.up;
      }
    }

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
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._dir_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._dir_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._dir_z);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._rot_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._rot_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(this.ParentEnvironment,
                                                          this,
                                                          this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
      //TODO: use envs bound extent if available for space

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
      if (this.ParentEnvironment == null) {
        return;
      }

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
      //TODO: Denormalize configuration if space is marked as normalised
      var transform1 = this.transform;
      var pos = transform1.position;
      var dir = transform1.forward;
      var rot = transform1.up;
      if (this.coordinate_space == CoordinateSpace.Environment_) {
        pos = this.ParentEnvironment.TransformPoint(pos);
        dir = this.ParentEnvironment.TransformDirection(dir);
        rot = this.ParentEnvironment.TransformDirection(rot);
      }

      var v = configuration.ConfigurableValue;
      if (this.PositionSpace.DecimalGranularity >= 0) {
        v = (int)Math.Round(v, this.PositionSpace.DecimalGranularity);
      }

      if (this.PositionSpace.Min[0].CompareTo(this.PositionSpace.Max[0]) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, PositionSpace.MinValues == PositionSpace.MaxValues, and use other space aswell!
        if (v < this.PositionSpace.Min[0] || v > this.PositionSpace.Max[0]) {
          Debug.Log(string.Format("Configurable does not accept input{2}, outside allowed range {0} to {1}",
                                  this.PositionSpace.Min[0],
                                  this.PositionSpace.Max[0],
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
      if (this.coordinate_space == CoordinateSpace.Environment_) {
        inv_pos = this.ParentEnvironment.InverseTransformPoint(pos);
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
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this.pos_space.Sample();
      var sample1 = this.rot_space.Sample();
      return new[] {
                       new Configuration(this._x, sample.x),
                       new Configuration(this._y, sample.y),
                       new Configuration(this._z, sample.z),
                       new Configuration(this._rot_x, sample1.x),
                       new Configuration(this._rot_y, sample1.y),
                       new Configuration(this._rot_z, sample1.z)
                   };
    }
  }
}
