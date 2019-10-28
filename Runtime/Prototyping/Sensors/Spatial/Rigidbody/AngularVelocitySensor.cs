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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "AngularVelocity"; } }

    public Vector3 ObservationValue {
      get { return this._angular_velocity; }
      set { this._angular_velocity = value; }
    }

    public Space3 TripleSpace { get { return this._angular_velocity_space; } }

    /// <summary>
    ///
    /// </summary>
    public override void PreSetup() { this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>(); }

    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z}; }
    }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() { this.ObservationValue = this._rigidbody.angularVelocity; }
  }
}
