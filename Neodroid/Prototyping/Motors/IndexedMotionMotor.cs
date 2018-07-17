using System;
using droid.Neodroid.Utilities.Messaging.Messages;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Motors {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "IndexedMotion" + MotorComponentMenuPath._Postfix)]
  public class IndexedMotionMotor : Motor {
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
    public override string PrototypingTypeName { get { return "IndexedMotion"; } }

    protected override void InnerApplyMotion(MotorMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);

      Vector3 vec;
      switch ((int)motion.Strength) {
        case 1:
          vec = Vector3.forward;
          break;
        case 2:
          vec = Vector3.back;
          break;
        case 3:
          vec = Vector3.left;
          break;
        case 4:
          vec = Vector3.right;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }

      if (this._No_Collisions) {
        if (!Physics.Raycast(this.transform.position, vec, Mathf.Abs(motion.Strength), layer_mask)) {
          this.transform.Translate(vec, this._Relative_To);
        }
      } else {
        this.transform.Translate(vec, this._Relative_To);
      }
    }
  }
}
