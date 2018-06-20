using System;
using droid.Neodroid.Utilities.Messaging.Messages;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Motors {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "BanditArm" + MotorComponentMenuPath._Postfix)]
  public class BanditArmMotor : Motor {

    [SerializeField] Material _material;

    /// <summary>
    /// 
    /// </summary>
    public override string PrototypingTypeName { get { return "BanditArm"; } }

    protected override void Setup() { this._material = this.GetComponent<Renderer>().sharedMaterial; }

    protected override void InnerApplyMotion(MotorMotion motion) {
     
      switch ((int)motion.Strength) {
        case 1:
          this._material.color = Color.blue;
          break;
        case 2:
          this._material.color = Color.black;
          break;
        case 3:
          this._material.color = Color.red;
          break;
        case 4:
          this._material.color = Color.green;
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}
