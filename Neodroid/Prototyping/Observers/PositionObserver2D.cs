using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "PositionObserver2D" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class PositionObserver2D : Observer,
                                                IHasDouble {
    [Header("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment_;

    [SerializeField] Vector2 _2_d_position;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(10);

    public ObservationSpace Space { get { return this._space; } }

    public override string PrototypingType { get { return "DoublePosition"; } }

    public Vector3 Position {
      get { return this._position; }
      set {
        this._position = this.NormaliseObservationUsingSpace
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
        this._2_d_position = new Vector2(this._position.x, this._position.z);
      }
    }

    public Vector2 ObservationValue { get { return this._2_d_position; } set { this._2_d_position = value; } }

    public Space2 ObservationSpace2D {
      get {
        return new Space2(this._position_space._Decimal_Granularity) {
            _Max_Values = new Vector2(this._position_space._Max_Values.x, this._position_space._Max_Values.y),
            _Min_Values = new Vector2(this._position_space._Min_Values.x, this._position_space._Min_Values.y)
        };
      }
    }

    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._space == ObservationSpace.Environment_) {
        this.Position = this.ParentEnvironment.TransformPosition(this.transform.position);
      } else if (this._space == ObservationSpace.Local_) {
        this.Position = this.transform.localPosition;
      } else {
        this.Position = this.transform.position;
      }

      this.FloatEnumerable = new[] {this.Position.x, this.Position.z};
    }

    protected override void PreSetup() { this.FloatEnumerable = new[] {this.Position.x, this.Position.z}; }
  }
}
