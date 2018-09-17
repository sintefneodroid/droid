using System;
using Neodroid.Utilities.Interfaces;
using Neodroid.Utilities.Structs;
using UnityEngine;

namespace Neodroid.Prototyping.Observers.Transform {
  [AddComponentMenu(PrototypingComponentMenuPath._ComponentMenuPath + "Observers/Position"),
   ExecuteInEditMode, Serializable]
  public class RotationObserver : Observer,
                                  IHasQuadruple {
    [Header("Specific", order = 102), SerializeField]
    ObservationSpace _space = ObservationSpace.Environment_;

    [Header("Observation", order = 103), SerializeField]
    Quaternion _rotation;

    /// <summary>
    /// 
    /// </summary>
    public ObservationSpace Space { get { return this._space; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Position"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void PreSetup() {
      this.FloatEnumerable =
          new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void UpdateObservation() {
      if (this.ParentEnvironment && this._space == ObservationSpace.Environment_) {
        this.ObservationValue = this.ParentEnvironment.TransformRotation(this.transform.rotation);
      } else if (this._space == ObservationSpace.Local_) {
        this.ObservationValue = this.transform.localRotation;
      } else {
        this.ObservationValue = this.transform.rotation;
      }

      this.FloatEnumerable =
          new[] {this.ObservationValue.x, this.ObservationValue.y, this.ObservationValue.z};
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Quaternion ObservationValue { get { return this._rotation; } set { this._rotation = value; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public Space4 QuadSpace { get; } = new Space4();
  }
}
