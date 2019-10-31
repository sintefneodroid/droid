using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Rotation")]
  [ExecuteInEditMode]
  [Serializable]
  public class RotationSensor : Sensor,
                                IHasQuadruple {
    [Header("Observation", order = 103)]
    [SerializeField]
    Quaternion _rotation;

    [Header("Specific", order = 102)]
    [SerializeField]
    CoordinateSpace _space = CoordinateSpace.Environment_;

    /// <summary>
    /// </summary>
    public CoordinateSpace Space { get { return this._space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Position"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Quaternion ObservationValue { get { return this._rotation; } set { this._rotation = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space4 QuadSpace { get; } = new Space4();



    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get { return new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z}; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this._space == CoordinateSpace.Environment_) {
        this.ObservationValue = this.ParentEnvironment.TransformRotation(this.transform.rotation);
      } else if (this._space == CoordinateSpace.Local_) {
        this.ObservationValue = this.transform.localRotation;
      } else {
        this.ObservationValue = this.transform.rotation;
      }
    }
  }
}
