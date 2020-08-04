using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Rigidbody {
  public class AngularVelocitySensor : Sensor,
                                       IHasTriple {
    [SerializeField] Vector3 _angular_velocity;
    [SerializeField] Space3 _angular_velocity_space = Space3.ZeroOne;

    [SerializeField] UnityEngine.Rigidbody _rigidbody;

    public Vector3 ObservationValue {
      get { return this._angular_velocity; }
      set { this._angular_velocity = value; }
    }

    public Space3 TripleSpace { get { return this._angular_velocity_space; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void PreSetup() { this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }

    public override IEnumerable<Single> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
        yield return this.ObservationValue.z;
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() { this.ObservationValue = this._rigidbody.angularVelocity; }
  }
}
