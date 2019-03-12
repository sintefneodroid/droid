using System;
using droid.Runtime.Utilities.Enums;
using droid.Runtime.Utilities.Misc.SearchableEnum;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ObserverComponentMenuPath._ComponentMenuPath
                    + "SingleAxisTransform"
                    + ObserverComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class SingleAxisTransformSensor : ValueSensor {
    [SerializeField] [SearchableEnum] Axis _dim = Axis.X_;

    /// <summary>
    /// </summary>
    protected override void PreSetup() { }

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
    }

    void OnDrawGizmos() {
      if (this.enabled) {
        switch (this._dim) {
          case Axis.Rot_x_:
          case Axis.X_:
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.right * 2, Color.green);
            break;
          case Axis.Rot_y_:
          case Axis.Y_:
            Debug.DrawLine(this.transform.position, this.transform.position + Vector3.up * 2, Color.green);
            break;
          case Axis.Rot_z_:
          case Axis.Z_:
            Debug.DrawLine(this.transform.position,
                           this.transform.position + Vector3.forward * 2,
                           Color.green);
            break;
          default: //TODO add the Direction cases
            Gizmos.DrawIcon(this.transform.position, "console.warnicon", true);
            break;
        }
      }
    }
  }
}
