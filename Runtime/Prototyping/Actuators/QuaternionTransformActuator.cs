using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "QuaternionTransform"
                    + ActuatorComponentMenuPath._Postfix)]
  public class QuaternionTransformActuator : Actuator {
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

    string _rot_x;
    string _rot_y;
    string _rot_z;

    string _x;
    string _y;
    string _z;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Transform"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this._x = this.Identifier + "X_";
      this._y = this.Identifier + "Y_";
      this._z = this.Identifier + "Z_";
      this._rot_x = this.Identifier + "RotX_";
      this._rot_y = this.Identifier + "RotY_";
      this._rot_z = this.Identifier + "RotZ_";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._x);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._y);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._z);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._rot_x);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._rot_y);
      this.Parent =
          NeodroidUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent, (Actuator)this, this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (motion.ActuatorName == this._x) {
        this.transform.Translate(Vector3.left * motion.Strength, this._Relative_To);
      } else if (motion.ActuatorName == this._y) {
        this.transform.Translate(-Vector3.up * motion.Strength, this._Relative_To);
      } else if (motion.ActuatorName == this._z) {
        this.transform.Translate(-Vector3.forward * motion.Strength, this._Relative_To);
      } else if (motion.ActuatorName == this._rot_x) {
        this.transform.Rotate(Vector3.left, motion.Strength, this._Relative_To);
      } else if (motion.ActuatorName == this._rot_y) {
        this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
      } else if (motion.ActuatorName == this._rot_z) {
        this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
      }
    }
  }
}
