using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                               + "Rigidbody3DofActuator"
                               + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(requiredComponent : typeof(Rigidbody))]
  public class Rigidbody3DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Angular_Actuators;

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
    string _x;

    /// <summary>
    /// </summary>
    string _y;

    /// <summary>
    /// </summary>
    string _z;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      if (this._Angular_Actuators) {
        this._x = this.Identifier + "RotX_";
        this._y = this.Identifier + "RotY_";
        this._z = this.Identifier + "RotZ_";
      }

      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                          c : this,
                                                          identifier : this._x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                          c : this,
                                                          identifier : this._y);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent(r : (IHasRegister<IActuator>)this.Parent,
                                                          c : this,
                                                          identifier : this._z);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(t : this, obj : this._x);
      this.Parent?.UnRegister(t : this, obj : this._y);
      this.Parent?.UnRegister(t : this, obj : this._z);
    }

    public override string[] InnerMotionNames => new[] {this._x, this._y, this._z};

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (!this._Angular_Actuators) {
        if (motion.ActuatorName == this._x) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(force : Vector3.right * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : Vector3.right * motion.Strength, mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._y) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(force : Vector3.up * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : Vector3.up * motion.Strength, mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._z) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(force : Vector3.forward * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(force : Vector3.forward * motion.Strength,
                                             mode : this._ForceMode);
          }
        }
      } else {
        if (motion.ActuatorName == this._x) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(torque : Vector3.right * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : Vector3.right * motion.Strength,
                                              mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._y) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(torque : Vector3.up * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : Vector3.up * motion.Strength, mode : this._ForceMode);
          }
        } else if (motion.ActuatorName == this._z) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(torque : Vector3.forward * motion.Strength, mode : this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : Vector3.forward * motion.Strength,
                                              mode : this._ForceMode);
          }
        }
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        if (this._Angular_Actuators) {
          Handles.DrawWireArc(center : this.transform.position,
                              normal : this.transform.right,
                              @from : -this.transform.forward,
                              180,
                              2);

          Handles.DrawWireArc(center : this.transform.position,
                              normal : this.transform.up,
                              @from : -this.transform.right,
                              180,
                              2);

          Handles.DrawWireArc(center : this.transform.position,
                              normal : this.transform.forward,
                              @from : -this.transform.right,
                              180,
                              2);
        } else {
          Debug.DrawLine(start : position, end : position + Vector3.right * 2, color : Color.green);

          Debug.DrawLine(start : position, end : position + Vector3.forward * 2, color : Color.green);

          Debug.DrawLine(start : position, end : position + Vector3.up * 2, color : Color.green);
        }
      }
    }
    #endif
  }
}
