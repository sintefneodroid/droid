using Neodroid.Runtime.Interfaces;
using Neodroid.Runtime.Prototyping.Actors;
using Neodroid.Runtime.Utilities.Misc;
using UnityEngine;

namespace Neodroid.Runtime.Prototyping.Motors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "RigidbodyMotor3Dof" + MotorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(Rigidbody))]
  public class RigidbodyMotor3Dof : Motor {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected bool _Angular_Motors;

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
      if (this._Angular_Motors) {
        this._x = this.Identifier + "RotX_";
        this._y = this.Identifier + "RotY_";
        this._z = this.Identifier + "RotZ_";
      }

      this.ParentActor = NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._x);
      this.ParentActor = NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._y);
      this.ParentActor = NeodroidUtilities.RegisterComponent((Actor)this.ParentActor, (Motor)this, this._z);
    }

    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotorMotion motion) {
      if (!this._Angular_Motors) {
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
