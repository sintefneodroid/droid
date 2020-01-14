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
    public ISamplable ConfigurableValueSpace {
      get {
        return this.pos_space;
        //return DirectionSpace;
        //return RotationSpace;
      }
    }

    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() { //TODO: IMPLEMENT LOCAL SPACE
      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_) {
        this.Position = this.ParentEnvironment.TransformPoint(point : this.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(direction : this.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(direction : this.transform.up);
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
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._z);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._dir_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._dir_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._dir_z);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._rot_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._rot_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          this,
                                                          identifier : this._rot_z);
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

      this.ParentEnvironment.UnRegister(this, identifier : this._x);
      this.ParentEnvironment.UnRegister(this, identifier : this._y);
      this.ParentEnvironment.UnRegister(this, identifier : this._z);
      this.ParentEnvironment.UnRegister(this, identifier : this._dir_x);
      this.ParentEnvironment.UnRegister(this, identifier : this._dir_y);
      this.ParentEnvironment.UnRegister(this, identifier : this._dir_z);
      this.ParentEnvironment.UnRegister(this, identifier : this._rot_x);
      this.ParentEnvironment.UnRegister(this, identifier : this._rot_y);
      this.ParentEnvironment.UnRegister(this, identifier : this._rot_z);
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
      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_) {
        pos = this.ParentEnvironment.TransformPoint(point : pos);
        dir = this.ParentEnvironment.TransformDirection(direction : dir);
        rot = this.ParentEnvironment.TransformDirection(direction : rot);
      }

      var v = configuration.ConfigurableValue;
      if (this.PositionSpace.DecimalGranularity >= 0) {
        v = (int)Math.Round(value : v, digits : this.PositionSpace.DecimalGranularity);
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
          pos.Set(v - pos.x, newY : pos.y, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(newX : pos.x, v - pos.y, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(newX : pos.x, newY : pos.y, v - pos.z);
        } else if (configuration.ConfigurableName == this._dir_x) {
          dir.Set(v - dir.x, newY : dir.y, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_y) {
          dir.Set(newX : dir.x, v - dir.y, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_z) {
          dir.Set(newX : dir.x, newY : dir.y, v - dir.z);
        } else if (configuration.ConfigurableName == this._rot_x) {
          rot.Set(v - rot.x, newY : rot.y, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_y) {
          rot.Set(newX : rot.x, v - rot.y, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_z) {
          rot.Set(newX : rot.x, newY : rot.y, v - rot.z);
        }
      } else {
        if (configuration.ConfigurableName == this._x) {
          pos.Set(newX : v, newY : pos.y, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._y) {
          pos.Set(newX : pos.x, newY : v, newZ : pos.z);
        } else if (configuration.ConfigurableName == this._z) {
          pos.Set(newX : pos.x, newY : pos.y, newZ : v);
        } else if (configuration.ConfigurableName == this._dir_x) {
          dir.Set(newX : v, newY : dir.y, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_y) {
          dir.Set(newX : dir.x, newY : v, newZ : dir.z);
        } else if (configuration.ConfigurableName == this._dir_z) {
          dir.Set(newX : dir.x, newY : dir.y, newZ : v);
        } else if (configuration.ConfigurableName == this._rot_x) {
          rot.Set(newX : v, newY : rot.y, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_y) {
          rot.Set(newX : rot.x, newY : v, newZ : rot.z);
        } else if (configuration.ConfigurableName == this._rot_z) {
          rot.Set(newX : rot.x, newY : rot.y, newZ : v);
        }
      }

      var inv_pos = pos;
      var inv_dir = dir;
      var inv_rot = rot;
      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_) {
        inv_pos = this.ParentEnvironment.InverseTransformPoint(point : pos);
        inv_dir = this.ParentEnvironment.InverseTransformDirection(direction : dir);
        inv_rot = this.ParentEnvironment.InverseTransformDirection(direction : rot);
      }

      this.transform.position = inv_pos;
      this.transform.rotation = Quaternion.identity;
      this.transform.rotation = Quaternion.LookRotation(forward : inv_dir, upwards : inv_rot);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this.pos_space.Sample();
      var sample1 = this.rot_space.Sample();
      return new[] {
                       new Configuration(configurable_name : this._x, configurable_value : sample.x),
                       new Configuration(configurable_name : this._y, configurable_value : sample.y),
                       new Configuration(configurable_name : this._z, configurable_value : sample.z),
                       new Configuration(configurable_name : this._rot_x, configurable_value : sample1.x),
                       new Configuration(configurable_name : this._rot_y, configurable_value : sample1.y),
                       new Configuration(configurable_name : this._rot_z, configurable_value : sample1.z)
                   };
    }
  }
}
