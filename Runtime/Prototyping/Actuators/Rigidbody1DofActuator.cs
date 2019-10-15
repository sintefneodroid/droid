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
    protected Axis _Axis_Of_Motion;

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
    public override string PrototypingTypeName { get { return "Rigidbody" + this._Axis_Of_Motion; } }

    /// <summary>
    /// </summary>
    public override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      switch (this._Axis_Of_Motion) {
        case Axis.X_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.right * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.right * motion.Strength, this._ForceMode);
          }

          break;
        case Axis.Y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.up * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength, this._ForceMode);
          }

          break;
        case Axis.Z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength, this._ForceMode);
          }

          break;
        case Axis.Rot_x_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.right * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.right * motion.Strength, this._ForceMode);
          }

          break;
        case Axis.Rot_y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength, this._ForceMode);
          }

          break;
        case Axis.Rot_z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength, this._ForceMode);
          }

          break;
        case Axis.Dir_x_: break;
        case Axis.Dir_y_: break;
        case Axis.Dir_z_: break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    public override string[] InnerMotionNames => new[] {this._Axis_Of_Motion.ToString()};

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
          case Axis.Dir_x_: break;
          case Axis.Dir_y_: break;
          case Axis.Dir_z_: break;
          default: //TODO add the Direction cases
            Gizmos.DrawIcon(position, "console.warnicon", true);
            break;
        }
      }
    }
    #endif
  }
}
