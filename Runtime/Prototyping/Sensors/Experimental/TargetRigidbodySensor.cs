using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actuators;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities.Structs;
using UnityEngine;
using UnityEngine.Serialization;

namespace droid.Runtime.Prototyping.Sensors.Experimental {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  public class TargetRigidbodySensor : Sensor,
                                       IHasDouble {

    [SerializeField]
    TargetRigidbodyActuator _actuator = null;

    [SerializeField] Space2 _observation_space2_d = Space2.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector2 ObservationValue {
      get { return new Vector2(this._actuator.MovementSpeed, this._actuator.RotationSpeed); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space2 DoubleSpace { get { return this._observation_space2_d; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      base.PreSetup();
      if (!this._actuator) {
        this._actuator = this.GetComponent<TargetRigidbodyActuator>();
      }
    }

    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.ObservationValue.x, this.ObservationValue.y}; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() { }
  }
}
