using System;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "BanditArm"
                    + ActuatorComponentMenuPath._Postfix)]
  public class BanditArmActuator : Actuator {
    [SerializeField] Material _material;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "BanditArm"; } }

    /// <summary>
    /// </summary>
    public override void Setup() {
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

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      if (this._material) {
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

    public override string[] InnerMotionNames =>
        new[] {
                  "1",
                  "2",
                  "3",
                  "4"
              };
  }
}
