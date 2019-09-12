using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "EulerTransformActuator1Dof"
                    + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransform1DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected Axis _Axis_Of_Motion;

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

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingTypeName { get { return "EulerTransform" + this._Axis_Of_Motion; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);
      var vec = Vector3.zero;
      switch (this._Axis_Of_Motion) {
        case Axis.X_: // Translational
          vec = Vector3.right * motion.Strength;
          break;
        case Axis.Y_: // Translational
          vec = -Vector3.up * motion.Strength;
          break;
        case Axis.Z_: // Translational
          vec = -Vector3.forward * motion.Strength;
          break;
        case Axis.Rot_x_: // Rotational
          this.transform.Rotate(Vector3.right, motion.Strength, this._Relative_To);
          return;
        case Axis.Rot_y_: // Rotational
          this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
          return;
        case Axis.Rot_z_: // Rotational
          this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
          return;
        case Axis.Dir_x_:
          this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
          return;
        case Axis.Dir_y_:
          this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
          return;
        case Axis.Dir_z_:
          this.transform.Rotate(Vector3.right, motion.Strength, this._Relative_To);
          return;
        default: throw new ArgumentOutOfRangeException();
      }

      if (this._No_Collisions) {
        if (!Physics.Raycast(this.transform.position,
                             vec,
                             Mathf.Abs(motion.Strength),
                             layer_mask)) {
          this.transform.Translate(vec, this._Relative_To);
        }
      } else {
        this.transform.Translate(vec, this._Relative_To);
      }
    }
    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._Axis_Of_Motion) {
          case Axis.X_:
            Debug.DrawLine(position, position + Vector3.right * 2, Color.green);
            break;
          case Axis.Y_:
            Debug.DrawLine(position, position + Vector3.up * 2, Color.green);
            break;
          case Axis.Z_:
            Debug.DrawLine(position, position + Vector3.forward * 2, Color.green);
            break;
          case Axis.Rot_x_:
            //Handles.DrawSolidArc
            //Handles.DrawSolidDisc

            Handles.DrawWireArc(this.transform.position,
                                this.transform.right,
                                -this.transform.forward,
                                180,
                                2);
            break;
          case Axis.Rot_y_:
            Handles.DrawWireArc(this.transform.position,
                                this.transform.up,
                                -this.transform.right,
                                180,
                                2);
            break;
          case Axis.Rot_z_:
            Handles.DrawWireArc(this.transform.position,
                                this.transform.forward,
                                -this.transform.right,
                                180,
                                2);
            break;
          case Axis.Dir_x_:
            Handles.DrawWireArc(this.transform.position,
                                this.transform.forward,
                                -this.transform.right,
                                180,
                                2);
            break;
          case Axis.Dir_y_:
            Handles.DrawWireArc(this.transform.position,
                                this.transform.up,
                                -this.transform.right,
                                180,
                                2);
            break;
          case Axis.Dir_z_:
            Handles.DrawWireArc(this.transform.position,
                                this.transform.right,
                                -this.transform.forward,
                                180,
                                2);
            break;
          default:
            Gizmos.DrawIcon(position, "console.warnicon", true);
            break;
        }
      }
    }

    #endif

    /// <summary>
    ///
    /// </summary>
    public override string[] InnerMotionNames { get { return new[] {this._Axis_Of_Motion.ToString()}; } }
  }
}
