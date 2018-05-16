using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  /// <summary>
  ///
  /// </summary>
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Compass" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class CompassObserver : Observer,
                                 IHasDouble {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Vector2 _2_d_position;

    /// <summary>
    ///
    /// </summary>
    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    Space3 _position_space = new Space3 {
        _Decimal_Granularity = 1,
        _Max_Values = Vector3.one,
        _Min_Values = -Vector3.one
    };

    /// <summary>
    ///
    /// </summary>
    [Header("Specfic", order = 102)]
    [SerializeField]
    Transform _target;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingType { get { return "Compass"; } }

    public Space2 ObservationSpace2D {
      get {
        return new Space2(this._position_space._Decimal_Granularity) {
            _Max_Values = new Vector2(this._position_space._Max_Values.x, this._position_space._Max_Values.y),
            _Min_Values = new Vector2(this._position_space._Min_Values.x, this._position_space._Min_Values.y)
        };
      }
    }

    /// <summary>
    ///
    /// </summary>
    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this.NormaliseObservationUsingSpace
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
        this._2_d_position = new Vector2(this._position.x, this._position.z);
      }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Vector2 ObservationValue { get { return this._2_d_position; } set { this._2_d_position = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() { this.FloatEnumerable = new[] {this.Position.x, this.Position.z}; }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      this.Position = this.transform.InverseTransformVector(this.transform.position - this._target.position)
          .normalized;

      this.FloatEnumerable = new[] {this.Position.x, this.Position.z};
    }
  }
}
