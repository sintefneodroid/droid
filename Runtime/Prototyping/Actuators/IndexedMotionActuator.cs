using System;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "IndexedMotion"
                    + ActuatorComponentMenuPath._Postfix)]
  public class IndexedMotionActuator : Actuator {
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
    public override string PrototypingTypeName { get { return "IndexedMotion"; } }

    /// <summary>
    ///
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
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

    public override string[] InnerMotionNames => new[] {"forward", "back", "left", "right"};
  }
}
