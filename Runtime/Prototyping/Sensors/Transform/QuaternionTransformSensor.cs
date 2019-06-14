using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Transform {
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
    ObservationSpace _space = ObservationSpace.Environment_;

    [SerializeField] bool _use_environments_coordinates = true;

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
      if (this.ParentEnvironment != null && this._use_environments_coordinates) {
        this._position = this.ParentEnvironment.TransformPoint(transform1.position);
        this._rotation = Quaternion.Euler(this.ParentEnvironment.TransformDirection(transform1.forward));
      } else {
        this._position = transform1.position;
        this._rotation = transform1.rotation;
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected override void PreSetup() { }
  }
}
