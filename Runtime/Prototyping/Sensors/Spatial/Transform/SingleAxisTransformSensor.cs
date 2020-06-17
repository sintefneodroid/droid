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
  [AddComponentMenu(menuName : SensorComponentMenuPath._ComponentMenuPath
                               + "SingleAxisTransform"
                               + SensorComponentMenuPath._Postfix)]
  [ExecuteInEditMode]
  public class SingleAxisTransformSensor : SingleValueSensor {
    [SerializeField] [SearchableEnum] AxisEnum _dim = AxisEnum.X_;

    [SerializeField] bool normalised_overwrite_space_if_env_bounds = true;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return base.PrototypingTypeName + this._dim; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void RemotePostSetup() {
      if (this.normalised_overwrite_space_if_env_bounds) {
        switch (this._dim) {
          case AxisEnum.X_:
            if (this.ParentEnvironment!=null && this.ParentEnvironment.PlayableArea!=null) {
              this._observation_value_space =
                  Space1.FromCenterExtent(extent : this.ParentEnvironment.PlayableArea.Bounds.extents.x);
            }

            break;
          case AxisEnum.Y_:
            if (this.ParentEnvironment!=null && this.ParentEnvironment.PlayableArea!=null) {
              this._observation_value_space =
                  Space1.FromCenterExtent(extent : this.ParentEnvironment.PlayableArea.Bounds.extents.y);
            }

            break;
          case AxisEnum.Z_:
            if (this.ParentEnvironment!=null && this.ParentEnvironment.PlayableArea!=null) {
              this._observation_value_space =
                  Space1.FromCenterExtent(extent : this.ParentEnvironment.PlayableArea.Bounds.extents.z);
            }

            break;
        }
      }
    }

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    ///  <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    public override void UpdateObservation() { //TODO: IMPLEMENT LOCAL SPACE
      switch (this._dim) {
        case AxisEnum.X_:
          this.ObservationValue = this.transform.position.x;
          break;
        case AxisEnum.Y_:
          this.ObservationValue = this.transform.position.y;
          break;
        case AxisEnum.Z_:
          this.ObservationValue = this.transform.position.z;
          break;
        case AxisEnum.Rot_x_:
          this.ObservationValue = this.transform.rotation.eulerAngles.x;
          break;
        case AxisEnum.Rot_y_:
          this.ObservationValue = this.transform.rotation.eulerAngles.y;
          break;
        case AxisEnum.Rot_z_:
          this.ObservationValue = this.transform.rotation.eulerAngles.z;
          break;
        case AxisEnum.Dir_x_:
          this.ObservationValue = this.transform.forward.x;
          break;
        case AxisEnum.Dir_y_:
          this.ObservationValue = this.transform.forward.y;
          break;
        case AxisEnum.Dir_z_:
          this.ObservationValue = this.transform.forward.z;
          break;
        default: throw new ArgumentOutOfRangeException();
      }
    }

    void OnDrawGizmos() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._dim) {
          case AxisEnum.Rot_x_:
          case AxisEnum.X_:

            Debug.DrawLine(start : position, end : position + Vector3.right * 2, color : Color.green);
            break;
          case AxisEnum.Rot_y_:
          case AxisEnum.Y_:

            Debug.DrawLine(start : position, end : position + Vector3.up * 2, color : Color.green);
            break;
          case AxisEnum.Rot_z_:
          case AxisEnum.Z_:
            Debug.DrawLine(start : position, end : position + Vector3.forward * 2, color : Color.green);
            break;
          case AxisEnum.Dir_x_: break;
          case AxisEnum.Dir_y_: break;
          case AxisEnum.Dir_z_: break;
          default: //TODO add the Direction cases
            Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }
  }
}
