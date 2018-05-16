using System;
using Neodroid.Utilities;
using UnityEngine;

namespace Neodroid.Prototyping.Motors {
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath
      + "EulerTransformMotor3Dof"
      + MotorComponentMenuPath._Postfix)]
  public class EulerTransformMotor3Dof : Motor {
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
    [SerializeField]
    protected bool _Rotational_Motors;

    /// <summary>
    ///
    /// </summary>
    [SerializeField]
    protected bool _Use_Mask = true;

    /// <summary>
    /// XAxisIdentifier
    /// </summary>
    string _x;

    /// <summary>
    /// YAxisIdentifier
    /// </summary>
    string _y;

    /// <summary>
    /// ZAxisIdentifier
    /// </summary>
    string _z;

    /// <summary>
    ///
    /// </summary>
    public override string PrototypingType { get { return "Transform"; } }

    /// <summary>
    ///
    /// </summary>
    protected override void Setup() {
      base.Setup();
      if (!this._Rotational_Motors) {
        this._x = this.Identifier + "X";
        this._y = this.Identifier + "Y";
        this._z = this.Identifier + "Z";
      } else {
        this._x = this.Identifier + "RotX";
        this._y = this.Identifier + "RotY";
        this._z = this.Identifier + "RotZ";
      }
    }

    /// <summary>
    ///
    /// </summary>
    protected override void RegisterComponent() {
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
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);
      if (!this._Rotational_Motors) {
        if (motion.MotorName == this._x) {
          var vec = Vector3.right * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        } else if (motion.MotorName == this._y) {
          var vec = -Vector3.up * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        } else if (motion.MotorName == this._z) {
          var vec = -Vector3.forward * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        }
      } else {
        if (motion.MotorName == this._x) {
          this.transform.Rotate(Vector3.left, motion.Strength, this._Relative_To);
        } else if (motion.MotorName == this._y) {
          this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
        } else if (motion.MotorName == this._z) {
          this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
        }
      }
    }
  }
}
