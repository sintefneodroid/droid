using System;
using droid.Runtime.Enums;
using droid.Runtime.Prototyping.Sensors.Experimental;
using droid.Runtime.Structs.Space;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Sensors.Spatial.Transform {
  /// <inheritdoc cref="Sensor" />
  /// <summary>
  /// </summary>
  [AddComponentMenu(SensorComponentMenuPath._ComponentMenuPath
                    + "SingleAxisTransform"
                    + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class SingleAxisTransformSensor : SingleValueSensor {
    [SerializeField] [SearchableEnum] Axis _dim = Axis.X_;

    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "SingleAxisTransform" + this._dim; } }

    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
    if(this.normalised_overwrite_space_if_env_bounds) {
        switch (this._dim) {
          case Axis.X_:
            if (this.ParentEnvironment) {
              this._observation_value_space =
                  Space1.FromCenterExtent(this.ParentEnvironment.PlayableArea.Bounds.extents.x);
            }

            break;
          case Axis.Y_:
            if (this.ParentEnvironment) {
              this._observation_value_space =
                  Space1.FromCenterExtent(this.ParentEnvironment.PlayableArea.Bounds.extents.y);
            }

            break;
          case Axis.Z_:
            if (this.ParentEnvironment) {
              this._observation_value_space =
                  Space1.FromCenterExtent(this.ParentEnvironment.PlayableArea.Bounds.extents.z);
            }
            break;
        }
      }
    }

    /// <summary>
    ///
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public override void UpdateObservation() { //TODO: IMPLEMENT LOCAL SPACE
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
        var position = this.transform.position;
        switch (this._dim) {
          case Axis.Rot_x_:
          case Axis.X_:

            Debug.DrawLine(position, position + Vector3.right * 2, Color.green);
            break;
          case Axis.Rot_y_:
          case Axis.Y_:

            Debug.DrawLine(position, position + Vector3.up * 2, Color.green);
            break;
          case Axis.Rot_z_:
          case Axis.Z_:
            Debug.DrawLine(position, position + Vector3.forward * 2, Color.green);
            break;
          case Axis.Dir_x_: break;
          case Axis.Dir_y_: break;
          case Axis.Dir_z_: break;
          default: //TODO add the Direction cases
            Gizmos.DrawIcon(position, "console.warnicon", true);
            break;
        }
      }
    }
  }
}
