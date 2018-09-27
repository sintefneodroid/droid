using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Prototyping.Motors;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  public class TargetRigidbodyObserver : Observer,
                                         IHasDouble {
    TargetRigidbodyMotor _motor;
    [SerializeField] Space2 _observation_space2_d;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      base.PreSetup();
      this._motor = this.GetComponent<TargetRigidbodyMotor>();
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.FloatEnumerable = new[] {this.ObservationValue.x, this.ObservationValue.y};
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector2 ObservationValue {
      get { return new Vector2(this._motor.MovementSpeed, this._motor.RotationSpeed); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space2 ObservationSpace2D { get { return this._observation_space2_d; } }
  }
}
