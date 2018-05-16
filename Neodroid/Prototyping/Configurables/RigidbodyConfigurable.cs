using System;
using Neodroid.Utilities;
using Neodroid.Utilities.Interfaces;
using UnityEngine;

namespace Neodroid.Prototyping.Configurables {
  /// <inheritdoc cref="ConfigurableGameObject" />
  ///  <summary>
  ///  </summary>
  [AddComponentMenu(
      ConfigurableComponentMenuPath._ComponentMenuPath
      + "Rigidbody"
      + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyConfigurable : ConfigurableGameObject,
                                       IHasRigidbody {
    /// <summary>
    ///
    /// </summary>
    Rigidbody _rigidbody;

    /// <summary>
    ///
    /// </summary>
    string _ang_x;

    /// <summary>
    ///
    /// </summary>
    string _ang_y;

    /// <summary>
    ///
    /// </summary>
    string _ang_z;

    /// <summary>
    ///
    /// </summary>
    string _vel_x;

    /// <summary>
    ///
    /// </summary>
    string _vel_y;

    /// <summary>
    ///
    /// </summary>
    string _vel_z;

    /// <summary>
    ///
    /// </summary>
    [Header("Observation", order = 110)]
    [SerializeField]
    Vector3 _velocity;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Utilities.Structs.Space3 _velocity_space;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector3 _angular_velocity;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Utilities.Structs.Space3 _angular_velocity_space;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "RigidbodyConfigurable"; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Velocity { get { return this._velocity; } set { this._velocity = value; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      private set { this._angular_velocity = value; }
    }

    /// <summary>
    ///
    /// </summary>
    public Utilities.Structs.Space3 VelocitySpace { get { return this._velocity_space; } }

    /// <summary>
    ///
    /// </summary>
    public Utilities.Structs.Space3 AngularSpace { get { return this._angular_velocity_space; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      this.Velocity = this._rigidbody.velocity;
      this.AngularVelocity = this._rigidbody.angularVelocity;
    }

    /// <summary>
    ///
    /// </summary>
    protected override void Setup() {
      this._rigidbody = this.GetComponent<Rigidbody>();
      this.UpdateCurrentConfiguration();
      this._vel_x = this.Identifier + "VelX";
      this._vel_y = this.Identifier + "VelY";
      this._vel_z = this.Identifier + "VelZ";
      this._ang_x = this.Identifier + "AngX";
      this._ang_y = this.Identifier + "AngY";
      this._ang_z = this.Identifier + "AngZ";
    }

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._vel_x);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._vel_y);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._vel_z);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._ang_x);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._ang_y);
      this.ParentEnvironment = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(
          this.ParentEnvironment,
          (ConfigurableGameObject)this,
          this._ang_z);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment) {
        this.ParentEnvironment.UnRegisterConfigurable(this._vel_x);
        this.ParentEnvironment.UnRegisterConfigurable(this._vel_y);
        this.ParentEnvironment.UnRegisterConfigurable(this._vel_z);
        this.ParentEnvironment.UnRegisterConfigurable(this._ang_x);
        this.ParentEnvironment.UnRegisterConfigurable(this._ang_y);
        this.ParentEnvironment.UnRegisterConfigurable(this._ang_z);
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="configuration"></param>
    public override void ApplyConfiguration(Utilities.Messaging.Messages.Configuration configuration) {
      var vel = this._rigidbody.velocity;
      var ang = this._rigidbody.velocity;

      var v = configuration.ConfigurableValue;
      if (this.VelocitySpace._Decimal_Granularity >= 0) {
        v = (int)Math.Round(v, this.VelocitySpace._Decimal_Granularity);
      }

      if (this.VelocitySpace._Min_Values[0].CompareTo(this.VelocitySpace._Max_Values[0]) != 0) {
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, VelocitySpace._Min_Values == VelocitySpace._Max_Values
        if (v < this.VelocitySpace._Min_Values[0] || v > this.VelocitySpace._Max_Values[0]) {
          Debug.Log(
              string.Format(
                  "Configurable does not accept input{2}, outside allowed range {0} to {1}",
                  this.VelocitySpace._Min_Values[0],
                  this.VelocitySpace._Max_Values[0],
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
        if (configuration.ConfigurableName == this._vel_x) {
          vel.Set(v - vel.x, vel.y, vel.z);
        } else if (configuration.ConfigurableName == this._vel_y) {
          vel.Set(vel.x, v - vel.y, vel.z);
        } else if (configuration.ConfigurableName == this._vel_z) {
          vel.Set(vel.x, vel.y, v - vel.z);
        } else if (configuration.ConfigurableName == this._ang_x) {
          ang.Set(v - ang.x, ang.y, ang.z);
        } else if (configuration.ConfigurableName == this._ang_y) {
          ang.Set(ang.x, v - ang.y, ang.z);
        } else if (configuration.ConfigurableName == this._ang_z) {
          ang.Set(ang.x, ang.y, v - ang.z);
        }
      } else {
        if (configuration.ConfigurableName == this._vel_x) {
          vel.Set(v, vel.y, vel.z);
        } else if (configuration.ConfigurableName == this._vel_y) {
          vel.Set(vel.x, v, vel.z);
        } else if (configuration.ConfigurableName == this._vel_z) {
          vel.Set(vel.x, vel.y, v);
        } else if (configuration.ConfigurableName == this._ang_x) {
          ang.Set(v, ang.y, ang.z);
        } else if (configuration.ConfigurableName == this._ang_y) {
          ang.Set(ang.x, v, ang.z);
        } else if (configuration.ConfigurableName == this._ang_z) {
          ang.Set(ang.x, ang.y, v);
        }
      }

      this._rigidbody.velocity = vel;
      this._rigidbody.angularVelocity = ang;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="random_generator"></param>
    /// <returns></returns>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override Utilities.Messaging.Messages.Configuration SampleConfiguration(
        System.Random random_generator) {
      throw new NotImplementedException();
    }
  }
}
