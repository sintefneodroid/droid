using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Misc.SearchableEnum;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Transform {
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

  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath + "EulerTransform" + ObserverComponentMenuPath._Postfix),
   ExecuteInEditMode, Serializable]
  public class EulerTransformObserver : Observer,
                                        IHasEulerTransform {
    [Header("Specific", order = 102), SerializeField, SearchableEnum]
    ObservationSpace _space = ObservationSpace.Environment_;

    [Header("Observation", order = 103), SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(10);

    [SerializeField] Vector3 _rotation;
    [SerializeField] Space3 _rotation_space = new Space3(10);

    [SerializeField] Vector3 _direction;
    [SerializeField] Space3 _direction_space = new Space3(10);

    public override string PrototypingTypeName {
      get { return "EulerTransform"; }
    }

    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this.NormaliseObservation ? this._position_space.ClipNormaliseRound(value) : value;
      }
    }

    public Vector3 Rotation {
      get { return this._rotation; }
      set {
        this._rotation = this.NormaliseObservation ? this._rotation_space.ClipNormaliseRound(value) : value;
      }
    }

    public Space3 PositionSpace { get; } = new Space3();
    public Space3 DirectionSpace { get; } = new Space3();
    public Space3 RotationSpace { get; } = new Space3();

    public Vector3 Direction {
      get { return this._direction; }
      set {
        this._direction = this.NormaliseObservation ? this._direction_space.ClipNormaliseRound(value) : value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this._space == ObservationSpace.Environment_) {
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