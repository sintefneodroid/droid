using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Rigidbody {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class VelocityObserver : Observer, IHasTriple {
    [SerializeField] Space3 _velocity_space = new Space3(10);
    [SerializeField] Vector3 _velocity;

    [SerializeField] UnityEngine.Rigidbody _rigidbody;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Rigidbody"; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._velocity; }
      set {
        this._velocity = this.NormaliseObservation ? this._velocity_space.ClipNormaliseRound(value) : value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateObservation() {
      this.ObservationValue = this._rigidbody.velocity;

      this.FloatEnumerable = new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this.FloatEnumerable = new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    public Space3 TripleSpace { get { return this._velocity_space; } }
  }
}