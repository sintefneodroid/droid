using droid.Runtime.Interfaces;
using droid.Runtime.Utilities;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "Rotation"
                    + ActuatorComponentMenuPath._Postfix)]
  public class RotationActuator : Actuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected Space _Relative_To = Space.Self;

    string _rot_x = "RotX";
    string _rot_y = "RotY";
    string _rot_z = "RotZ";

    string _rot_w = "RotW";
    //new Space1 MotionSpace = Space1.ZeroOne;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Rotation"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._rot_x = this.Identifier + "RotX";
      this._rot_y = this.Identifier + "RotY";
      this._rot_z = this.Identifier + "RotZ";
      this._rot_w = this.Identifier + "RotW";
    }

    public override string[] InnerMotionNames =>
        new[] {
                  this._rot_x,
                  this._rot_y,
                  this._rot_z,
                  this._rot_w
              };

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          this._rot_x);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          this._rot_y);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          this._rot_z);
      this.Parent =
          NeodroidRegistrationUtilities.RegisterComponent((IHasRegister<IActuator>)this.Parent,
                                                          (Actuator)this,
                                                          this._rot_w);
    }

    /// <summary>
    ///
    /// </summary>
    protected override void UnRegisterComponent() {
      this.Parent?.UnRegister(this, this._rot_x);
      this.Parent?.UnRegister(this, this._rot_y);
      this.Parent?.UnRegister(this, this._rot_z);
      this.Parent?.UnRegister(this, this._rot_w);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      var transform_rotation = this.transform.rotation;
      if (motion.ActuatorName == this._rot_x) {
        transform_rotation.x = motion.Strength;
      } else if (motion.ActuatorName == this._rot_y) {
        transform_rotation.y = motion.Strength;
      } else if (motion.ActuatorName == this._rot_z) {
        transform_rotation.z = motion.Strength;
      } else if (motion.ActuatorName == this._rot_w) {
        transform_rotation.z = motion.Strength;
      }

      this.transform.rotation = transform_rotation;
    }
  }
}
