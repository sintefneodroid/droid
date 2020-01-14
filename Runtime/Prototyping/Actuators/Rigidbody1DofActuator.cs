using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "Rigidbody1DofActuator"
                    + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class Rigidbody1DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [Header("General", order = 101)]
    [SerializeField]
    protected AxisEnum _axisEnum_of_motion;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected ForceMode _ForceMode = ForceMode.Force;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Rigidbody _Rigidbody;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Rigidbody" + this._axisEnum_of_motion; } }

    /// <summary>
    /// </summary>
    public override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      switch (this._axisEnum_of_motion) {
        case AxisEnum.X_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.right * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.right * motion.Strength, mode : this._ForceMode);
          }

          break;
        case AxisEnum.Y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.up * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength, mode : this._ForceMode);
          }

          break;
        case AxisEnum.Z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength, mode : this._ForceMode);
          }

          break;
        case AxisEnum.Rot_x_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.right * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.right * motion.Strength, mode : this._ForceMode);
          }

          break;
        case AxisEnum.Rot_y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength, mode : this._ForceMode);
          }

          break;
        case AxisEnum.Rot_z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength, mode : this._ForceMode);
          }

          break;
        case AxisEnum.Dir_x_: break;
        case AxisEnum.Dir_y_: break;
        case AxisEnum.Dir_z_: break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public override string[] InnerMotionNames => new[] {this._axisEnum_of_motion.ToString()};

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        switch (this._axisEnum_of_motion) {
          case AxisEnum.X_:
            Debug.DrawLine(start : position, position + Vector3.right * 2, color : Color.green);
            break;
          case AxisEnum.Y_:
            Debug.DrawLine(start : position, position + Vector3.up * 2, color : Color.green);
            break;
          case AxisEnum.Z_:
            Debug.DrawLine(start : position, position + Vector3.forward * 2, color : Color.green);
            break;
          case AxisEnum.Rot_x_:
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
          case AxisEnum.Dir_x_: break;
          case AxisEnum.Dir_y_: break;
          case AxisEnum.Dir_z_: break;
          default: //TODO add the Direction cases
            Gizmos.DrawIcon(center : position, "console.warnicon", true);
            break;
        }
      }
    }
    #endif
  }
}
