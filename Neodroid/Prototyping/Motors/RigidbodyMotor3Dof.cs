using System;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "RigidbodyMotor3Dof" + MotorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyMotor3Dof : Motor {
    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected Rigidbody _Rigidbody;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected ForceMode _ForceMode = ForceMode.Force;

    [SerializeField] protected bool _Rotational_Motors;

    /// <summary>
    ///
    /// </summary>
    string _x;

    /// <summary>
    ///
    /// </summary>
    string _y;

    /// <summary>
    ///
    /// </summary>
    string _z;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "Rigidbody"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() { this._Rigidbody = this.GetComponent<Rigidbody>(); }

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
      if (this._Rotational_Motors) {
        this._x = this.Identifier + "RotX";
        this._y = this.Identifier + "RotY";
        this._z = this.Identifier + "RotZ";
      }

      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._x);
      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._y);
      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._z);
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) {
      if (!this._Rotational_Motors) {
        if (motion.MotorName == this._x) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.left * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.left * motion.Strength, this._ForceMode);
          }
        } else if (motion.MotorName == this._y) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.up * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength, this._ForceMode);
          }
        } else if (motion.MotorName == this._z) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength, this._ForceMode);
          }
        }
      } else {
        if (motion.MotorName == this._x) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.left * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.left * motion.Strength, this._ForceMode);
          }
        } else if (motion.MotorName == this._y) {
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength, this._ForceMode);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength, this._ForceMode);
          }
        } else if (motion.MotorName == this._z) {
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
