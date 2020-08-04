using System;
using System.Collections.Generic;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using droid.Runtime.Structs.Space;
using UnityEngine;
using Object = System.Object;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
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
    CoordinateSpaceEnum _spaceEnum = CoordinateSpaceEnum.Environment_;

    [SerializeField] Space3 _position_space = Space3.MinusOneOne;
    [SerializeField] Space4 _rotation_space = Space4.MinusOneOne;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Vector3 Position { get { return this._position; } }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public Quaternion Rotation { get { return this._rotation; } }

    public Space3 PositionSpace { get { return this._position_space; } } //TODO: Implement
    public Space4 RotationSpace { get{ return this._rotation_space; } } //TODO: Implement

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override IEnumerable<Single> FloatEnumerable {
      get {
        yield return this._position.x;
        yield return this._position.y;
        yield return this._position.z;
        yield return this._rotation.x;
        yield return this._rotation.y;
        yield return this._rotation.z;
        yield return this._rotation.w;
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override void UpdateObservation() {
      var transform1 = this.transform;
      if (this.ParentEnvironment != null && this._spaceEnum == CoordinateSpaceEnum.Environment_) {
        this._position = this.ParentEnvironment.TransformPoint(point : transform1.position);
        this._rotation =
            Quaternion.Euler(euler : this.ParentEnvironment.TransformDirection(direction : transform1.up));
      } else {
        this._position = transform1.position;
        this._rotation = transform1.rotation;
      }
    }
  }
}
