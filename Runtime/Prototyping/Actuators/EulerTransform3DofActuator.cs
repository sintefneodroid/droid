using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "EulerTransform3DofActuator"
                    + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransform3DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected string _Layer_Mask = "Obstructions";

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _No_Collisions = true;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Rotational_Actuators;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Use_Mask = true;

    /// <summary>
    ///   XAxisIdentifier
    /// </summary>
    string _x;

    /// <summary>
    ///   YAxisIdentifier
    /// </summary>
    string _y;

    /// <summary>
    ///   ZAxisIdentifier
    /// </summary>
    string _z;

    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Transform"; } }

    /// <summary>
    /// </summary>
    protected override void Setup() {
      if (!this._Rotational_Actuators) {
        this._x = this.Identifier + "X_";
        this._y = this.Identifier + "Y_";
        this._z = this.Identifier + "Z_";
      } else {
        this._x = this.Identifier + "RotX_";
        this._y = this.Identifier + "RotY_";
        this._z = this.Identifier + "RotZ_";
      }
    }

    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, this, this._x);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, this, this._y);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, this, this._z);
    }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);
      if (!this._Rotational_Actuators) {
        if (motion.ActuatorName == this._x) {
          var vec = Vector3.right * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        } else if (motion.ActuatorName == this._y) {
          var vec = -Vector3.up * motion.Strength;
          if (this._No_Collisions) {
            if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask)) {
              this.transform.Translate(vec, this._Relative_To);
            }
          } else {
            this.transform.Translate(vec, this._Relative_To);
          }
        } else if (motion.ActuatorName == this._z) {
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
        if (motion.ActuatorName == this._x) {
          this.transform.Rotate(Vector3.left, motion.Strength, this._Relative_To);
        } else if (motion.ActuatorName == this._y) {
          this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
        } else if (motion.ActuatorName == this._z) {
          this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
        }
      }
    }
  }
}
