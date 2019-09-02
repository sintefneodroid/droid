using System;
using System.Collections.Generic;
using droid.Runtime.Interfaces;
using droid.Runtime.Sampling;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Transform {
  /// <summary>
  /// </summary>
  public enum ObservationSpace {
    /// <summary>
    /// </summary>
    Local_,

    /// <summary>
    /// </summary>
    Global_,

    /// <summary>
    /// </summary>
    Environment_
  }

  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "EulerTransform"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class EulerTransformSensor : Sensor,
                                      IHasEulerTransform {
    [SerializeField] Vector3 _direction;
    [SerializeField] Space3 _direction_space = new Space3( 10);

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3( 10);

    [SerializeField] Vector3 _rotation;
    [SerializeField] Space3 _rotation_space = new Space3( 10);

    [Header("Specific", order = 102)]
    [SerializeField]
    [SearchableEnum]
    ObservationSpace _space = ObservationSpace.Environment_;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName { get { return "EulerTransform"; } }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this._position_space.Normalised
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Rotation {
      get { return this._rotation; }
      set {
        this._rotation = this._rotation_space.Normalised
                             ? this._rotation_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Space3 PositionSpace { get; } = new Space3();

    /// <summary>
    ///
    /// </summary>
    public Space3 DirectionSpace { get; } = new Space3();

    /// <summary>
    ///
    /// </summary>
    public Space3 RotationSpace { get; } = new Space3();

    /// <summary>
    ///
    /// </summary>
    public Vector3 Direction {
      get { return this._direction; }
      set {
        this._direction = this._direction_space.Normalised
                              ? this._direction_space.ClipNormaliseRound(value)
                              : value;
      }
    }

    /// <summary>
    ///
    /// </summary>
    public override IEnumerable<float> FloatEnumerable {
      get {
        return new[] {
                         this.Position.x,
                         this.Position.y,
                         this.Position.z,
                         this.Direction.x,
                         this.Direction.y,
                         this.Direction.z,
                         this.Rotation.x,
                         this.Rotation.y,
                         this.Rotation.z
                     };
      }
    }

    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      var transform1 = this.transform;
      if (this.ParentEnvironment != null && this._space == ObservationSpace.Environment_) {
        this.Position = this.ParentEnvironment.TransformPoint(transform1.position);
        this.Direction = this.ParentEnvironment.TransformDirection(transform1.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(transform1.up);
      } else if (this._space == ObservationSpace.Local_) {
        this.Position = transform1.localPosition;
        this.Direction = transform1.forward;
        this.Rotation = transform1.up;
      } else {
        this.Position = transform1.position;
        this.Direction = transform1.forward;
        this.Rotation = transform1.up;
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected override void PreSetup() { }
  }
}
