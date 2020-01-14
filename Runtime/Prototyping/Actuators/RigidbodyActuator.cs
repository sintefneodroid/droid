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
                    + "Rigidbody"
                    + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyActuator : Actuator {
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

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    public override string PrototypingTypeName { get { return "Rigidbody"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._Rigidbody = this.GetComponent<Rigidbody>();

      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      this._rot_x = this.Identifier + "RotX_";
      this._rot_y = this.Identifier + "RotY_";
      this._rot_z = this.Identifier + "RotZ_";
    }

    public override string[] InnerMotionNames =>
        new[] {
                  this._x,
                  this._y,
                  this._z,
                  this._rot_x,
                  this._rot_y,
                  this._rot_z
              };

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      //this.ParentActor = NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.ParentActor, (Actuator)this);

      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          identifier : this._x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          identifier : this._y);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          identifier : this._z);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          identifier : this._rot_x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          identifier : this._rot_y);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          identifier : this._rot_z);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(this, obj : this._x);
      this.Parent?.UnRegister(this, obj : this._y);
      this.Parent?.UnRegister(this, obj : this._z);

      this.Parent?.UnRegister(this, obj : this._rot_x);
      this.Parent?.UnRegister(this, obj : this._rot_y);
      this.Parent?.UnRegister(this, obj : this._rot_z);
    }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (this._Relative_To == Space.World) {
        if (motion.ActuatorName == this._x) {
          this._Rigidbody.AddForce(Vector3.right * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._y) {
          this._Rigidbody.AddForce(Vector3.up * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._z) {
          this._Rigidbody.AddForce(Vector3.forward * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_x) {
          this._Rigidbody.AddTorque(Vector3.right * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_y) {
          this._Rigidbody.AddTorque(Vector3.up * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_z) {
          this._Rigidbody.AddTorque(Vector3.forward * motion.Strength, mode : this._ForceMode);
        }
      } else if (this._Relative_To == Space.Self) {
        if (motion.ActuatorName == this._x) {
          this._Rigidbody.AddRelativeForce(Vector3.right * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._y) {
          this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._z) {
          this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_x) {
          this._Rigidbody.AddRelativeTorque(Vector3.right * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_y) {
          this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength, mode : this._ForceMode);
        } else if (motion.ActuatorName == this._rot_z) {
          this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength, mode : this._ForceMode);
        }
      } else {
        Debug.LogWarning($"Not applying force in space {this._Relative_To}");
      }
    }

    #if UNITY_EDITOR
    void OnDrawGizmosSelected() {
      if (this.enabled) {
        var position = this.transform.position;

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

        Debug.DrawLine(start : position, position + Vector3.right * 2, color : Color.green);

        Debug.DrawLine(start : position, position + Vector3.forward * 2, color : Color.green);

        Debug.DrawLine(start : position, position + Vector3.up * 2, color : Color.green);
      }
    }
    #endif
  }
}
