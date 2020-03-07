using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Prototyping.Configurables.Transforms;
using droid.Runtime.Structs.Space;
using droid.Runtime.Structs.Space.Sample;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables.Rigidbody {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                               + "Rigidbody"
                               + ConfigurableComponentMenuPath._Postfix)]
  [RequireComponent(requiredComponent : typeof(UnityEngine.Rigidbody))]
  public class RigidbodyConfigurable : SpatialConfigurable,
                                       IHasRigidbody {
    /// <summary>
    /// </summary>
    string _ang_x;

    /// <summary>
    /// </summary>
    string _ang_y;

    /// <summary>
    /// </summary>
    string _ang_z;

    /// <summary>
    /// </summary>
    [SerializeField]
    Vector3 _angular_velocity = Vector3.zero;

    /// <summary>
    /// </summary>
    [SerializeField]
    SampleSpace3 _angular_velocity_space = new SampleSpace3 {_space = Space3.ZeroOne};

    /// <summary>
    /// </summary>
    UnityEngine.Rigidbody _rigidbody = null;

    /// <summary>
    /// </summary>
    string _vel_x;

    /// <summary>
    /// </summary>
    string _vel_y;

    /// <summary>
    /// </summary>
    string _vel_z;

    /// <summary>
    /// </summary>
    [Header("Observation", order = 110)]
    [SerializeField]
    Vector3 _velocity = Vector3.zero;

    /// <summary>
    /// </summary>
    [SerializeField]
    SampleSpace3 _velocity_space = new SampleSpace3 {_space = Space3.ZeroOne};

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "RigidbodyConfigurable"; } }

    /// <summary>
    /// </summary>
    public Vector3 Velocity { get { return this._velocity; } set { this._velocity = value; } }

    /// <summary>
    /// </summary>
    public Vector3 AngularVelocity {
      get { return this._angular_velocity; }
      private set { this._angular_velocity = value; }
    }

    public Space3 VelocitySpace { get { return this._velocity_space._space; } }
    public Space3 AngularSpace { get { return this._angular_velocity_space._space; } }

    public ISamplable ConfigurableValueSpace { get; }

    /// <summary>
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      this.Velocity = this._rigidbody.velocity;
      this.AngularVelocity = this._rigidbody.angularVelocity;
    }

    /// <summary>
    /// </summary>
    public override void PreSetup() {
      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this._vel_x = this.Identifier + "VelX";
      this._vel_y = this.Identifier + "VelY";
      this._vel_z = this.Identifier + "VelZ";
      this._ang_x = this.Identifier + "AngX";
      this._ang_y = this.Identifier + "AngY";
      this._ang_z = this.Identifier + "AngZ";
    }

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._vel_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._vel_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._vel_z);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._ang_x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._ang_y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._ang_z);
    }

    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(t : this, identifier : this._vel_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._vel_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._vel_z);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._ang_x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._ang_y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._ang_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      var vel = this._rigidbody.velocity;
      var ang = this._rigidbody.velocity;

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._vel_x) {
          var v = this.VelocitySpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).x;
          vel.Set(newX : v - vel.x, newY : vel.y, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_y) {
          var v = this.VelocitySpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).y;
          vel.Set(newX : vel.x, newY : v - vel.y, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_z) {
          var v = this.VelocitySpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).z;
          vel.Set(newX : vel.x, newY : vel.y, newZ : v - vel.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_x) {
          var v = this.AngularSpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).x;
          ang.Set(newX : v - ang.x, newY : ang.y, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_y) {
          var v = this.AngularSpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).y;
          ang.Set(newX : ang.x, newY : v - ang.y, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_z) {
          var v = this.AngularSpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).z;
          ang.Set(newX : ang.x, newY : ang.y, newZ : v - ang.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._vel_x) {
          var v = this.VelocitySpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).x;
          vel.Set(newX : v, newY : vel.y, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_y) {
          var v = this.VelocitySpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).y;
          vel.Set(newX : vel.x, newY : v, newZ : vel.z);
        } else if (simulator_configuration.ConfigurableName == this._vel_z) {
          var v = this.VelocitySpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).z;
          vel.Set(newX : vel.x, newY : vel.y, newZ : v);
        } else if (simulator_configuration.ConfigurableName == this._ang_x) {
          var v = this.AngularSpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).x;
          ang.Set(newX : v, newY : ang.y, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_y) {
          var v = this.AngularSpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).y;
          ang.Set(newX : ang.x, newY : v, newZ : ang.z);
        } else if (simulator_configuration.ConfigurableName == this._ang_z) {
          var v = this.AngularSpace.Reproject(v : Vector3.one * simulator_configuration.ConfigurableValue).z;
          ang.Set(newX : ang.x, newY : ang.y, newZ : v);
        }
      }

      this._rigidbody.velocity = vel;
      this._rigidbody.angularVelocity = ang;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// <exception cref="T:System.NotImplementedException"></exception>
    public override Configuration[] SampleConfigurations() {
      return new[] {
                       new Configuration(configurable_name : this._ang_z,
                                         configurable_value : this._angular_velocity_space.Sample().z)
                   };
    }
  }
}
