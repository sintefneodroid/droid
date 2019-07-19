using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Rigidbody {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [RequireComponent(typeof(UnityEngine.Rigidbody))]
  public class VelocitySensor : Sensor,
                                IHasTriple {
    [SerializeField] UnityEngine.Rigidbody _rigidbody;
    [SerializeField] Vector3 _velocity;
    [SerializeField] Space3 _velocity_space = new Space3(new DistributionSampler(), 10);

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Rigidbody"; } }

    /// <summary>
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._velocity; }
      set {
        this._velocity = this._velocity_space.Normalised
                             ? this._velocity_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space3 TripleSpace { get { return this._velocity_space; } }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z}; }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() { this.ObservationValue = this._rigidbody.velocity; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {


      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
    }
  }
}
