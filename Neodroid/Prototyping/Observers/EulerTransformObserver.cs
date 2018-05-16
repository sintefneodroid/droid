using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  /// <summary>
  ///
  /// </summary>
  public enum ObservationSpace {
    /// <summary>
    ///
    /// </summary>
    Local_,

    /// <summary>
    ///
    /// </summary>
    Global_,

    /// <summary>
    ///
    /// </summary>
    Environment_
  }

  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "EulerTransform" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class EulerTransformObserver : Observer,
                                        IHasEulerTransform {
    [Header("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment_;

    [SerializeField] Vector3 _direction;
    [SerializeField] Space3 _direction_space = new Space3(10);

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(10);
    [SerializeField] Vector3 _rotation;
    [SerializeField] Space3 _rotation_space = new Space3(10);

    public ObservationSpace Space { get { return this._space; } }

    public override string PrototypingType { get { return "EulerTransform"; } }

    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this.NormaliseObservationUsingSpace
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    public Vector3 Rotation {
      get { return this._rotation; }
      set {
        this._rotation = this.NormaliseObservationUsingSpace
                             ? this._rotation_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    public Space3 PositionSpace { get; }
    public Space3 DirectionSpace { get; }
    public Space3 RotationSpace { get; }

    public Vector3 Direction {
      get { return this._direction; }
      set {
        this._direction = this.NormaliseObservationUsingSpace
                              ? this._direction_space.ClipNormaliseRound(value)
                              : value;
      }
    }

    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._space == ObservationSpace.Environment_) {
        this.Position = this.ParentEnvironment.TransformPosition(this.transform.position);
        this.Direction = this.ParentEnvironment.TransformDirection(this.transform.forward);
        this.Rotation = this.ParentEnvironment.TransformDirection(this.transform.up);
      } else if (this._space == ObservationSpace.Local_) {
        this.Position = this.transform.localPosition;
        this.Direction = this.transform.forward;
        this.Rotation = this.transform.up;
      } else {
        this.Position = this.transform.position;
        this.Direction = this.transform.forward;
        this.Rotation = this.transform.up;
      }

      this.FloatEnumerable = new[] {
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

    protected override void PreSetup() {
      this.FloatEnumerable = new[] {
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
}
