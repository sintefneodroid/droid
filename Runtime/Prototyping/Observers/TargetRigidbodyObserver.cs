using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Motors;
using droid.Runtime.Utilities.Structs;
using UnityEngine;

namespace droid.Runtime.Prototyping.Observers {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  public class TargetRigidbodyObserver : Observer,
                                         IHasDouble {
    [SerializeField] TargetRigidbodyMotor _motor = null;
    [SerializeField] Space2 _observation_space2_d = Space2.ZeroOne;

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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      base.PreSetup();
      if (!this._motor) {
        this._motor = this.GetComponent<TargetRigidbodyMotor>();
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
