using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
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

    [SerializeField] Space3 _position_space = Space3.ZeroOne;

    [Header("Specific", order = 102)]
    [SerializeField]
    CoordinateSpace _space = CoordinateSpace.Environment_;

    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Position"; } }

    /// <summary>
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._position; }
      set {
        this._position = this._position_space.Project(value);
      }
    }

    /// <summary>
    /// </summary>
    public Space3 TripleSpace { get { return this._position_space; } }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment) {
          this._position_space = Space3.FromCenterExtents(this.ParentEnvironment.PlayableArea.Bounds.extents);
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z}; }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this._space == CoordinateSpace.Environment_) {
        this.ObservationValue = this.ParentEnvironment.TransformPoint(this.transform.position);
      } else if (this._space == CoordinateSpace.Local_) {
        this.ObservationValue = this.transform.localPosition;
      } else {
        this.ObservationValue = this.transform.position;
      }
    }
  }
}
