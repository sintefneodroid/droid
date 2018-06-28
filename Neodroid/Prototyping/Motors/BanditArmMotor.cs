using System;
using droid.Neodroid.Utilities.Messaging.Messages;
using UnityEngine;

namespace droid.Neodroid.Prototyping.Motors {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "BanditArm" + MotorComponentMenuPath._Postfix)]
  public class BanditArmMotor : Motor {

    [SerializeField] Material _material;

    /// <summary>
    /// 
    /// </summary>
    public override string PrototypingTypeName { get { return "BanditArm"; } }

    protected override void Setup() {
      var renderr = this.GetComponent<Renderer>();
      if (renderr) {
        this._material = renderr.sharedMaterial;
      } else {
        var rendr = this.GetComponent<CanvasRenderer>();
        if (rendr) {
          this._material = rendr.GetMaterial();
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(MotorMotion motion) {
     if(this._material) {
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
}
