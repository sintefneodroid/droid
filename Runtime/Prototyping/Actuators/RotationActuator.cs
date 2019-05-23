using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.Misc;
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

    string _rot_x="RotX";
    string _rot_y="RotY";
    string _rot_z="RotZ";
    string _rot_w="RotW";
    //new Space1 MotionSpace = Space1.ZeroOne;


    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Rotation"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() {
      this._rot_x = this.Identifier + "RotX";
      this._rot_y = this.Identifier + "RotY";
      this._rot_z = this.Identifier + "RotZ";
      this._rot_w = this.Identifier + "RotW";
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void RegisterComponent() {
      this.ParentActor =
          NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Actuator)this, this._rot_x);
      this.ParentActor =
          NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Actuator)this, this._rot_y);
      this.ParentActor =
          NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Actuator)this, this._rot_z);
      this.ParentActor =
          NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Actuator)this, this._rot_w);
    }

    protected override void UnRegisterComponent() {
      this.ParentActor?.UnRegister(this, this._rot_x);
      this.ParentActor?.UnRegister(this, this._rot_y);
      this.ParentActor?.UnRegister(this, this._rot_z);
      this.ParentActor?.UnRegister(this, this._rot_w);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      var transformRotation = this.transform.rotation;
      if (motion.ActuatorName == this._rot_x) {

        transformRotation.x = motion.Strength;

      } else if (motion.ActuatorName == this._rot_y) {
        transformRotation.y = motion.Strength;
      } else if (motion.ActuatorName == this._rot_z) {
        transformRotation.z = motion.Strength;
      } else if (motion.ActuatorName == this._rot_w) {
        transformRotation.z = motion.Strength;
      }
      this.transform.rotation = transformRotation;
    }
    }
  }

