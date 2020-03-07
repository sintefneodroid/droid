using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  [AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                               + "EulerTransformActuator1Dof"
                               + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransform1DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected AxisEnum _axisEnum_of_motion;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected string _Layer_Mask = "Obstructions";

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected bool _No_Collisions = true;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <inheritdoc />
    ///  <summary>
    ///  </summary>
    public override string PrototypingTypeName {
      get { return base.PrototypingTypeName + this._axisEnum_of_motion; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(layerName : this._Layer_Mask);
      var vec = Vector3.zero;
      switch (this._axisEnum_of_motion) {
        case AxisEnum.X_: // Translational
          vec = Vector3.right * motion.Strength;
          break;
        case AxisEnum.Y_: // Translational
          vec = -Vector3.up * motion.Strength;
          break;
        case AxisEnum.Z_: // Translational
          vec = -Vector3.forward * motion.Strength;
          break;
        case AxisEnum.Rot_x_: // Rotational
          this.transform.Rotate(axis : Vector3.right,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case AxisEnum.Rot_y_: // Rotational
          this.transform.Rotate(axis : Vector3.up, angle : motion.Strength, relativeTo : this._Relative_To);
          return;
        case AxisEnum.Rot_z_: // Rotational
          this.transform.Rotate(axis : Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case AxisEnum.Dir_x_:
          this.transform.Rotate(axis : Vector3.forward,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        case AxisEnum.Dir_y_:
          this.transform.Rotate(axis : Vector3.up, angle : motion.Strength, relativeTo : this._Relative_To);
          return;
        case AxisEnum.Dir_z_:
          this.transform.Rotate(axis : Vector3.right,
                                angle : motion.Strength,
                                relativeTo : this._Relative_To);
          return;
        default: throw new ArgumentOutOfRangeException();
      }

      if (this._No_Collisions) {
        if (!Physics.Raycast(origin : this.transform.position,
                             direction : vec,
                             maxDistance : Mathf.Abs(f : motion.Strength),
                             layerMask : layer_mask)) {
          this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
        }
      } else {
        this.transform.Translate(translation : vec, relativeTo : this._Relative_To);
      }
    }
    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._axisEnum_of_motion) {
          case AxisEnum.X_:
            Debug.DrawLine(start : position, end : position + Vector3.right * 2, color : Color.green);
            break;
          case AxisEnum.Y_:
            Debug.DrawLine(start : position, end : position + Vector3.up * 2, color : Color.green);
            break;
          case AxisEnum.Z_:
            Debug.DrawLine(start : position, end : position + Vector3.forward * 2, color : Color.green);
            break;
          case AxisEnum.Rot_x_:
            //Handles.DrawSolidArc
            //Handles.DrawSolidDisc

            Handles.DrawWireArc(center : this.transform.position,
                                normal : this.transform.right,
                                @from : -this.transform.forward,
                                180,
                                2);
            break;
          case AxisEnum.Rot_y_:
            Handles.DrawWireArc(center : this.transform.position,
                                normal : this.transform.up,
                                @from : -this.transform.right,
                                180,
                                2);
            break;
          case AxisEnum.Rot_z_:
            Handles.DrawWireArc(center : this.transform.position,
                                normal : this.transform.forward,
                                @from : -this.transform.right,
                                180,
                                2);
            break;
          case AxisEnum.Dir_x_:
            Handles.DrawWireArc(center : this.transform.position,
                                normal : this.transform.forward,
                                @from : -this.transform.right,
                                180,
                                2);
            break;
          case AxisEnum.Dir_y_:
            Handles.DrawWireArc(center : this.transform.position,
                                normal : this.transform.up,
                                @from : -this.transform.right,
                                180,
                                2);
            break;
          case AxisEnum.Dir_z_:
            Handles.DrawWireArc(center : this.transform.position,
                                normal : this.transform.right,
                                @from : -this.transform.forward,
                                180,
                                2);
            break;
          default:
            Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }

    #endif

    /// <summary>
    ///
    /// </summary>
    public override string[] InnerMotionNames { get { return new[] {this._axisEnum_of_motion.ToString()}; } }
  }
}
