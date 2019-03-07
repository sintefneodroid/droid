using droid.Runtime.Interfaces;
using droid.Runtime.Prototyping.Actors;
using droid.Runtime.Utilities.Misc;
using UnityEngine;

namespace droid.Runtime.Prototyping.Motors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(MotorComponentMenuPath._ComponentMenuPath
                    + "EulerTransform"
                    + MotorComponentMenuPath._Postfix)]
  public class EulerTransformMotor : Motor {
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
      this.ParentActor = NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._x);
      this.ParentActor = NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._y);
      this.ParentActor = NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._z);
      this.ParentActor =
          NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._rot_x);
      this.ParentActor =
          NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._rot_y);
      this.ParentActor =
          NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._rot_z);
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotorMotion motion) {
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
