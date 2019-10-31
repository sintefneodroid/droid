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
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "Rigidbody3DofActuator"
                    + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
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

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Rigidbody"; } }

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
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          this,
                                                          this._x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          this,
                                                          this._y);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          this,
                                                          this._z);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(this, this._x);
      this.Parent?.UnRegister(this, this._y);
      this.Parent?.UnRegister(this, this._z);
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
            this._Rigidbody.AddForce(Vector3.right * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.right * motion.Strength, this._ForceMode);
          }
        } else if (motion.ActuatorName == this._y) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.up * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength, this._ForceMode);
          }
        } else if (motion.ActuatorName == this._z) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength, this._ForceMode);
          }
        }
      } else {
        if (motion.ActuatorName == this._x) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.right * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.right * motion.Strength, this._ForceMode);
          }
        } else if (motion.ActuatorName == this._y) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength, this._ForceMode);
          }
        } else if (motion.ActuatorName == this._z) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength, this._ForceMode);
          }
        }
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;
        if (this._Angular_Actuators) {
          Handles.DrawWireArc(this.transform.position,
                              this.transform.right,
                              -this.transform.forward,
                              180,
                              2);

          Handles.DrawWireArc(this.transform.position,
                              this.transform.up,
                              -this.transform.right,
                              180,
                              2);

          Handles.DrawWireArc(this.transform.position,
                              this.transform.forward,
                              -this.transform.right,
                              180,
                              2);
        } else {
          Debug.DrawLine(position, position + Vector3.right * 2, Color.green);

          Debug.DrawLine(position, position + Vector3.forward * 2, Color.green);

          Debug.DrawLine(position, position + Vector3.up * 2, Color.green);
        }
      }
    }
    #endif
  }
}
