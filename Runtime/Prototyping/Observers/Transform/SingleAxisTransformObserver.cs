using System;
using Neodroid.Runtime.Utilities.Enums;
using Neodroid.Runtime.Utilities.Misc.SearchableEnum;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Observers.Transform {
  /// <inheritdoc cref="Observer" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
       ObserverComponentMenuPath._ComponentMenuPath
       + "SingleAxisTransform"
       + ObserverComponentMenuPath._Postfix), ExecuteInEditMode]
  public class SingleAxisTransformObserver : ValueObserver {

    [SerializeField, SearchableEnum] Axis _dim = Axis.X_;

    /// <summary>
    /// 
    /// </summary>
    protected override void PreSetup() { this.FloatEnumerable = new[] {this.ObservationValue}; }

    public override void UpdateObservation() {
      switch (this._dim) {
        case Axis.X_:
          this.ObservationValue = this.transform.position.x;
          break;
        case Axis.Y_:
          this.ObservationValue = this.transform.position.y;
          break;
        case Axis.Z_:
          this.ObservationValue = this.transform.position.z;
          break;
        case Axis.Rot_x_:
          this.ObservationValue = this.transform.rotation.eulerAngles.x;
          break;
        case Axis.Rot_y_:
          this.ObservationValue = this.transform.rotation.eulerAngles.y;
          break;
        case Axis.Rot_z_:
          this.ObservationValue = this.transform.rotation.eulerAngles.z;
          break;
        case Axis.Dir_x_:
          this.ObservationValue = this.transform.forward.x;
          break;
        case Axis.Dir_y_:
          this.ObservationValue = this.transform.forward.y;
          break;
        case Axis.Dir_z_:
          this.ObservationValue = this.transform.forward.z;
          break;
        default: throw new ArgumentOutOfRangeException();
      }

      this.FloatEnumerable = new[] {this.ObservationValue};
    }

    void OnDrawGizmos() {
      if (this.enabled) {
        switch (this._dim) {
          case Axis.X_:
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.right * 2, Color.green);

            break;
          case Axis.Y_:
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.right * 2, Color.green);
            break;
          case Axis.Z_:
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.up * 2, Color.green);
            break;
          default: throw new ArgumentOutOfRangeException();
        }
      }
    }
  }
}
