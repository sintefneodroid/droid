using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers {
  [AddComponentMenu(
      ObserverComponentMenuPath._ComponentMenuPath + "Position" + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  [Serializable]
  public class PositionObserver : Observer,
                                  IHasTriple {
    [Header("Specfic", order = 102)]
    [SerializeField]
    ObservationSpace _space = ObservationSpace.Environment_;

    [Header("Observation", order = 103)]
    [SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(10);

    public ObservationSpace Space { get { return this._space; } }

    public override string PrototypingType { get { return "Position"; } }

    public Vector3 ObservationValue {
      get { return this._position; }
      set {
        this._position = this.NormaliseObservationUsingSpace
                             ? this._position_space.ClipNormaliseRound(value)
                             : value;
      }
    }

    public Space3 TripleSpace { get; }

    protected override void PreSetup() {
      this.FloatEnumerable =
          new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._space == ObservationSpace.Environment_) {
        this.ObservationValue = this.ParentEnvironment.TransformPosition(this.transform.position);
      } else if (this._space == ObservationSpace.Local_) {
        this.ObservationValue = this.transform.localPosition;
      } else {
        this.ObservationValue = this.transform.position;
      }

      this.FloatEnumerable =
          new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }
  }
}
