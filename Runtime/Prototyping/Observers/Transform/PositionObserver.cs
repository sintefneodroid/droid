using System;
using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Transform {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath + "Position" + ObserverComponentMenuPath._Postfix),
   ExecuteInEditMode, Serializable]
  public class PositionObserver : Observer,
                                  IHasTriple {
    [Header("Specific", order = 102), SerializeField]
    ObservationSpace _space = ObservationSpace.Environment_;

    [Header("Observation", order = 103), SerializeField]
    Vector3 _position;

    [SerializeField] Space3 _position_space = new Space3(10);

    /// <summary>
    /// 
    /// </summary>
    public override string PrototypingTypeName {
      get { return "Position"; }
    }

    /// <summary>
    /// 
    /// </summary>
    public Vector3 ObservationValue {
      get { return this._position; }
      set {
        this._position = this.NormaliseObservation ? this._position_space.ClipNormaliseRound(value) : value;
      }
    }

    /// <summary>
    /// 
    /// </summary>
    public Space3 TripleSpace { get; } = new Space3();

    /// <summary>
    /// 
    /// </summary>
    protected override void PreSetup() {
      this.FloatEnumerable =
          new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    /// <summary>
    /// 
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment != null && this._space == ObservationSpace.Environment_) {
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