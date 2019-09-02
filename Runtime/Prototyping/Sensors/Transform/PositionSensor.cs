using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      SensorComponentMenuPath._ComponentMenuPath + "Position" + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class PositionSensor : Sensor,
                                IHasTriple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3( 10);

    [Header("Specific", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment_;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Position"; } }

    /// <summary>
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._position; }
      set {
        this._position = this._position_space.Normalised
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    /// <summary>
    /// </summary>
    public Space3 TripleSpace { get { return this._position_space; } }

    /// <summary>
    /// </summary>
    protected override void PreSetup() { }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z}; }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this._space == ObservationSpace.Environment_) {
        this.ObservationValue = this.ParentEnvironment.TransformPoint(this.transform.position);
      } else if (this._space == ObservationSpace.Local_) {
        this.ObservationValue = this.transform.localPosition;
      } else {
        this.ObservationValue = this.transform.position;
      }
    }
  }
}
