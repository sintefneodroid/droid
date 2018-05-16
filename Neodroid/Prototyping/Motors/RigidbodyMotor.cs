using System;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "Rigidbody" + MotorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyMotor : Motor {
    [SerializeField] protected Space _Relative_To = Space.Self;

    [SerializeField] protected Rigidbody _Rigidbody;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected ForceMode _ForceMode = ForceMode.Force;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    public override string PrototypingType { get { return "Rigidbody"; } }

    protected override void Setup() {
      this._Rigidbody = this.GetComponent<Rigidbody>();

      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
      this._rot_x = this.Identifier + "RotX";
      this._rot_y = this.Identifier + "RotY";
      this._rot_z = this.Identifier + "RotZ";
    }

    protected override void RegisterComponent() {
      this.ParentActor = Utilities.Unsorted.NeodroidUtilities.MaybeRegisterComponent(this.ParentActor, (Motor)this);

      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._x);
      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._y);
      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._z);
      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_x);
      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_y);
      this.ParentActor =
          Utilities.Unsorted.NeodroidUtilities.MaybeRegisterNamedComponent(this.ParentActor, (Motor)this, this._rot_z);
    }

    protected override void InnerApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) {
      if (this._Relative_To == Space.World) {
        if (motion.MotorName == this._x) {
          this._Rigidbody.AddForce(Vector3.left * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._y) {
          this._Rigidbody.AddForce(Vector3.up * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._z) {
          this._Rigidbody.AddForce(Vector3.forward * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._rot_x) {
          this._Rigidbody.AddTorque(Vector3.left * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._rot_y) {
          this._Rigidbody.AddTorque(Vector3.up * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._rot_z) {
          this._Rigidbody.AddTorque(Vector3.forward * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._x) {
          this._Rigidbody.AddRelativeForce(Vector3.left * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._y) {
          this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._z) {
          this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._rot_x) {
          this._Rigidbody.AddRelativeTorque(Vector3.left * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._rot_y) {
          this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength, this._ForceMode);
        } else if (motion.MotorName == this._rot_z) {
          this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength, this._ForceMode);
        }
      }
    }
  }
}
