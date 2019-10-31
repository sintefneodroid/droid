using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "QuaternionTransform"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class QuaternionTransformSensor : Sensor,
                                           IHasQuaternionTransform {
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Quaternion _rotation;

    [Header("Specific", order = 102)]
    [SerializeField]
    CoordinateSpace _space = CoordinateSpace.Environment_;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName { get { return "QuaternionTransform"; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Position { get { return this._position; } }

    /// <summary>
    ///
    /// </summary>
    public Quaternion Rotation { get { return this._rotation; } }

    public Space1 PositionSpace { get; } //TODO: Implement
    public Space1 RotationSpace { get; } //TODO: Implement

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get {
        return new[] {
                         this._position.x,
                         this._position.y,
                         this._position.z,
                         this._rotation.x,
                         this._rotation.y,
                         this._rotation.z,
                         this._rotation.w
                     };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override void UpdateObservation() {
      var transform1 = this.transform;
      if (this.ParentEnvironment != null && this._space == CoordinateSpace.Environment_) {
        this._position = this.ParentEnvironment.TransformPoint(transform1.position);
        this._rotation = Quaternion.Euler(this.ParentEnvironment.TransformDirection(transform1.forward));
      } else {
        this._position = transform1.position;
        this._rotation = transform1.rotation;
      }
    }


  }
}
