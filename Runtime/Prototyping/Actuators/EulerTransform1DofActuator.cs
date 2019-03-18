﻿using System;
using droid.Runtime.Interfaces;
using droid.Runtime.Utilities.Enums;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators {
  /// <inheritdoc />
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "EulerTransformActuator1Dof"
                    + ActuatorComponentMenuPath._Postfix)]
  public class EulerTransform1DofActuator : Actuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    protected Axis _Axis_Of_Motion;

    [SerializeField] protected string _Layer_Mask = "Obstructions";

    [SerializeField] protected bool _No_Collisions = true;

    [SerializeField] protected Space _Relative_To = Space.Self;

    public override string PrototypingTypeName { get { return "Transform" + this._Axis_Of_Motion; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    /// <exception cref="T:System.ArgumentOutOfRangeException"></exception>
    protected override void InnerApplyMotion(IMotion motion) {
      var layer_mask = 1 << LayerMask.NameToLayer(this._Layer_Mask);
      var vec = Vector3.zero;
      switch (this._Axis_Of_Motion) {
        case Axis.X_: // Translational
          vec = Vector3.right * motion.Strength;
          break;
        case Axis.Y_: // Translational
          vec = -Vector3.up * motion.Strength;
          break;
        case Axis.Z_: // Translational
          vec = -Vector3.forward * motion.Strength;
          break;
        case Axis.Rot_x_: // Rotational
          this.transform.Rotate(Vector3.left, motion.Strength, this._Relative_To);
          break;
        case Axis.Rot_y_: // Rotational
          this.transform.Rotate(Vector3.up, motion.Strength, this._Relative_To);
          break;
        case Axis.Rot_z_: // Rotational
          this.transform.Rotate(Vector3.forward, motion.Strength, this._Relative_To);
          break;
        case Axis.Dir_x_: break;
        case Axis.Dir_y_: break;
        case Axis.Dir_z_: break;
        default: throw new ArgumentOutOfRangeException();
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
