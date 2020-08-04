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
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "Position"
                               + SensorComponentMenuPath._Postfix)]
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
    CoordinateSpaceEnum _spaceEnum = CoordinateSpaceEnum.Environment_;

    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._position; }
      set { this._position = this._position_space.Project(v : value); }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space3 TripleSpace { get { return this._position_space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        if (this.ParentEnvironment) {
          this._position_space =
              Space3.FromCenterExtents(bounds_extents : this.ParentEnvironment.PlayableArea.Bounds.extents);
        }
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<Single> FloatEnumerable {
      get {
        yield return this.ObservationValue.x;
        yield return this.ObservationValue.y;
        yield return this.ObservationValue.z;
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this._spaceEnum == CoordinateSpaceEnum.Environment_) {
        this.ObservationValue = this.ParentEnvironment.TransformPoint(point : this.transform.position);
      } else if (this._spaceEnum == CoordinateSpaceEnum.Local_) {
        this.ObservationValue = this.transform.localPosition;
      } else {
        this.ObservationValue = this.transform.position;
      }
    }
  }
}
