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
  [AddComponentMenu(menuName : ConfigurableComponentMenuPath._ComponentMenuPath
                               + "EulerRotation"
                               + ConfigurableComponentMenuPath._Postfix)]
  public class EulerRotationConfigurable : SpatialConfigurable,
                                           IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Quaternion _euler_rotation = Quaternion.identity;

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
    SampleSpace3 _euler_space = new SampleSpace3 {
                                                     _space = new Space3 {
                                                                             Min = Vector3.zero,
                                                                             Max = new Vector3(360f,
                                                                                               360f,
                                                                                               360f)
                                                                         }
                                                 };

    /// <summary>
    ///
    /// </summary>
    public Space3 TripleSpace { get { return (Space3)this._euler_space.Space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void PreSetup() {
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
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment, c : (Configurable)this);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._x);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._y);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._z);
      this.ParentEnvironment =
          NeodroidRegistrationUtilities.RegisterComponent(r : this.ParentEnvironment,
                                                          c : (Configurable)this,
                                                          identifier : this._w);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void UnRegisterComponent() {
      if (this.ParentEnvironment == null) {
        return;
      }

      this.ParentEnvironment.UnRegister(configurable : this);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._x);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._y);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._z);
      this.ParentEnvironment.UnRegister(t : this, identifier : this._w);
    }

    /// <summary>
    ///
    /// </summary>
    public ISamplable ConfigurableValueSpace { get { return this._euler_space; } }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateCurrentConfiguration() {
      switch (this._coordinate_spaceEnum) {
        case CoordinateSpaceEnum.Environment_ when this.ParentEnvironment != null:
          this._euler_rotation =
              this.ParentEnvironment.TransformRotation(quaternion : this.transform.rotation);
          break;
        case CoordinateSpaceEnum.Global_:
          this._euler_rotation = this.transform.rotation;
          break;
        case CoordinateSpaceEnum.Local_:
          this._euler_rotation = this.transform.localRotation;
          break;
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="simulator_configuration"></param>
    public override void ApplyConfiguration(IConfigurableConfiguration simulator_configuration) {
      Quaternion rot;
      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Local_) {
        rot = this.transform.localRotation;
      } else {
        rot = this.transform.rotation;
      }

      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_) {
        rot = this.ParentEnvironment.TransformRotation(quaternion : this.transform.rotation);
      }

      var v = simulator_configuration.ConfigurableValue;
      if (this.TripleSpace.DecimalGranularity >= 0) {
        v = (int)Math.Round(value : v, digits : this.TripleSpace.DecimalGranularity);
      }

      #if NEODROID_DEBUG
      if (this.Debugging) {
        Debug.Log(message : $"Applying {v} to {simulator_configuration.ConfigurableName} configurable");
      }
      #endif
      var rote = rot.eulerAngles;

      if (this.RelativeToExistingValue) {
        if (simulator_configuration.ConfigurableName == this._x) {
          if (this.TripleSpace.Min[0].CompareTo(this.TripleSpace.Max[0]) != 0) {
            #if NEODROID_DEBUG
            if (v < this.TripleSpace.Min[0] || v > this.TripleSpace.Max[0]) {
              Debug.Log(message :
                        $"Configurable does not accept input {v}, outside allowed range {this.TripleSpace.Min[0]} to {this.TripleSpace.Max[0]}");
              return; // Do nothing
            }
            #endif
          }

          rot.eulerAngles = new Vector3(x : v + rote.x, y : rote.y, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          rot.eulerAngles = new Vector3(x : rote.x, y : v + rote.y, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          rot.eulerAngles = new Vector3(x : rote.x, y : rote.y, z : v + rote.z);
        }
      } else {
        if (simulator_configuration.ConfigurableName == this._x) {
          rot.eulerAngles = new Vector3(x : v, y : rote.y, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._y) {
          rot.eulerAngles = new Vector3(x : rote.x, y : v, z : rote.z);
        } else if (simulator_configuration.ConfigurableName == this._z) {
          rot.eulerAngles = new Vector3(x : rote.x, y : rote.y, z : v);
        }
      }

      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Environment_) {
        rot = this.ParentEnvironment.InverseTransformRotation(quaternion : rot);
      }

      if (this._coordinate_spaceEnum == CoordinateSpaceEnum.Local_) {
        this.transform.localRotation = rot;
      } else {
        this.transform.rotation = rot;
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <returns></returns>
    public override Configuration[] SampleConfigurations() {
      var sample = this._euler_space.Sample();

      return new[] {
                       new Configuration(configurable_name : this._x, configurable_value : sample.x),
                       new Configuration(configurable_name : this._y, configurable_value : sample.y),
                       new Configuration(configurable_name : this._z, configurable_value : sample.z)
                   };
    }

    Vector3 IHasTriple.ObservationValue { get { return this._euler_rotation.eulerAngles; } }
  }
}
