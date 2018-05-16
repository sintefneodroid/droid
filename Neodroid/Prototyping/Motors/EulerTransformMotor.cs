using System;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "EulerTransform" + MotorComponentMenuPath._Postfix)]
  public class EulerTransformMotor : Motor {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    protected string _Layer_Mask = "Obstructions";

    [SerializeField] protected bool _No_Collisions = true;

    [SerializeField] protected Space _Relative_To = Space.Self;

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    protected override void Setup() {
      base.Setup();
      this._x = this.Identifier + "X";
      this._y = this.Identifier + "Y";
      this._z = this.Identifier + "Z";
      this._rot_x = this.Identifier + "RotX";
      this._rot_y = this.Identifier + "RotY";
      this._rot_z = this.Identifier + "RotZ";
    }

    public override string PrototypingType { get { return "Transform"; } }

    protected override void RegisterComponent() {
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
      if (motion.MotorName == this._x) {
        this.transform.Translate(Vector3.left * motion.Strength, this._Relative_To);
      } else if (motion.MotorName == this._y) {
        this.transform.Translate(-Vector3.up * motion.Strength, this._Relative_To);
      } else if (motion.MotorName == this._z) {
        this.transform.Translate(-Vector3.forward * motion.Strength, this._Relative_To);
      } else if (motion.MotorName == this._rot_x) {
        this.transform.Rotate(Vector3.left, motion.Strength, this._Relative_To);
      } else if (motion.MotorName == this._rot_y) {
        this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
      } else if (motion.MotorName == this._rot_z) {
        this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
      }
    }
  }
}
