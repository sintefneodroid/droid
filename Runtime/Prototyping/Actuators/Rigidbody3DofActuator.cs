using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

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
    protected override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

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
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, this, this._x);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, this, this._y);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, this, this._z);
    }


    /// <summary>
    ///
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(this, this._x);
      this.Parent?.UnRegister(this, this._y);
      this.Parent?.UnRegister(this, this._z);

    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (!this._Angular_Actuators) {
        if (motion.ActuatorName == this._x) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.left * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.left * motion.Strength, this._ForceMode);
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
            this._Rigidbody.AddTorque(Vector3.left * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.left * motion.Strength, this._ForceMode);
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
  }
}
