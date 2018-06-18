using droid.Neodroid.Utilities.Structs;
using UnityEngine;
using Object = System.Object;

namespace droid.Neodroid.Prototyping.Observers.Rigidbody {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  public class VelocityObserver : Observer {
    [SerializeField] Space3 _velocity_space = new Space3(10);
    Vector3 _velocity;

    [SerializeField] UnityEngine.Rigidbody _rigidbody;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Rigidbody"; } }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 Velocity {
      get { return this._velocity; }
      set {
        this._velocity = this.NormaliseObservation ? this._velocity_space.ClipNormaliseRound(value) : value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateObservation() {
      this.Velocity = this._rigidbody.velocity;

      this.FloatEnumerable = new[] {this.Velocity.x, this.Velocity.y, this.Velocity.z,};
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this._rigidbody = this.GetComponent<UnityEngine.Rigidbody>();
      this.FloatEnumerable = new[] {this.Velocity.x, this.Velocity.y, this.Velocity.z};
    }
  }
}
