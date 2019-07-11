using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Messaging.Messages;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Misc;
using droid.Runtime.Utilities.Sampling;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Configurables {
  /// <inheritdoc cref="Configurable" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ConfigurableComponentMenuPath._ComponentMenuPath
                    + "EulerRotation"
                    + ConfigurableComponentMenuPath._Postfix)]
  public class EulerRotationConfigurable : Configurable,
                                           IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Quaternion _euler_rotation = Quaternion.identity;

    [SerializeField] bool _use_environments_space = false;

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

    [SerializeField]
    Space3 _euler_space =
        new Space3(new DistributionSampler(), 2) {
                                                     MinValues = Vector3.zero,
                                                     MaxValues = new Vector3(360f, 360f, 360f)
                                                 };

    public Space3 TripleSpace { get { return this._euler_space; } }

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
          NeodroidUtilities.RegisterComponent(this.ParentEnvironment, (Configurable)this);
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

      this.ParentEnvironment.UnRegister(this);
      this.ParentEnvironment.UnRegister(this, this._x);
      this.ParentEnvironment.UnRegister(this, this._y);
      this.ParentEnvironment.UnRegister(this, this._z);
      this.ParentEnvironment.UnRegister(this, this._w);
    }

    public override ISpace ConfigurableValueSpace { get; }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      if (this._use_environments_space && this.ParentEnvironment != null) {
        this._euler_rotation = this.ParentEnvironment.TransformRotation(this.transform.rotation);
      } else {
        this._euler_rotation = this.transform.rotation;
      }
    }

    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      var rot = this.transform.rotation;
      if (this._use_environments_space) {
        rot = this.ParentEnvironment.TransformRotation(this.transform.rotation);
      }

      var v = simulator_configuration.ConfigurableValue;
      if (this.TripleSpace.DecimalGranularity >= 0) {
        v = (int)Math.Round(v, this.TripleSpace.DecimalGranularity);
      }

      if (this.TripleSpace.MinValues[0].CompareTo(this.TripleSpace.MaxValues[0]) != 0) {
        #if NEODROID_DEBUG
        //TODO NOT IMPLEMENTED CORRECTLY VelocitySpace should not be index but should check all pairwise values, TripleSpace.MinValues == TripleSpace.MaxValues
        if (v < this.TripleSpace.MinValues[0] || v > this.TripleSpace.MaxValues[0]) {
          Debug.Log($"Configurable does not accept input {v}, outside allowed range {this.TripleSpace.MinValues[0]} to {this.TripleSpace.MaxValues[0]}");
          return; // Do nothing
        }
        #endif
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log($"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif
      var rote = rot.eulerAngles;

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          rot.eulerAngles = new Vector3(v - rote.x, rote.y, rote.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          rot.eulerAngles = new Vector3(rote.x, v - rote.y, rote.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          rot.eulerAngles = new Vector3(rote.x, rote.y, v - rote.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          rot.eulerAngles = new Vector3(v, rote.y, rote.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          rot.eulerAngles = new Vector3(rote.x, v, rote.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          rot.eulerAngles = new Vector3(rote.x, rote.y, v);
        }
      }

      if (this._use_environments_space) {
        rot = this.ParentEnvironment.InverseTransformRotation(rot);
      }

      this.transform.rotation = rot;
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this.TripleSpace.Sample();

      return new[] {
                       new Configuration(this._x, sample.x),
                       new Configuration(this._y, sample.y),
                       new Configuration(this._z, sample.z)
                   };
    }

    Vector3 IHasTriple.ObservationValue { get { return this._euler_rotation.eulerAngles; } }
  }
}
