using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators.Particles {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(menuName : ActuatorComponentMenuPath._ComponentMenuPath
                               + "Particles/Rocket"
                               + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(requiredComponent : typeof(ParticleSystem))]
  [RequireComponent(requiredComponent : typeof(Rigidbody))]
  public class RocketActuator : Rigidbody1DofActuator {
    /// <summary>
    /// </summary>
    [SerializeField]
    bool _fired_this_step;

    /// <summary>
    /// </summary>
    [SerializeField]
    protected ParticleSystem _Particle_System;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName {
      get { return base.PrototypingTypeName + this._axisEnum_of_motion; }
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() {
      this._Rigidbody = this.GetComponent<Rigidbody>();
      this._Particle_System = this.GetComponent<ParticleSystem>();
      var valid_input = this.MotionSpace;
      valid_input.Min = 0;
      this.MotionSpace = valid_input;
    }

    /// <summary>
    /// </summary>
    void LateUpdate() {
      if (this._fired_this_step) {
        if (!this._Particle_System.isPlaying) {
          this._Particle_System.Play(true);
        }
      } else {
        this._Particle_System.Stop(true);
      }

      this._fired_this_step = false;
    }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(IMotion motion) {
      if (motion.Strength < this.MotionSpace.Min || motion.Strength > this.MotionSpace.Max) {
        Debug.Log(message :
                  $"It does not accept input {motion.Strength}, outside allowed range {this.MotionSpace.Min} to {this.MotionSpace.Max}");
        return; // Do nothing
      }

      switch (this._axisEnum_of_motion) {
        case AxisEnum.X_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(force : Vector3.right * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(force : Vector3.right * motion.Strength);
          }

          break;
        case AxisEnum.Y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(force : Vector3.up * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(force : Vector3.up * motion.Strength);
          }

          break;
        case AxisEnum.Z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(force : Vector3.forward * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(force : Vector3.forward * motion.Strength);
          }

          break;
        case AxisEnum.Rot_x_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(torque : Vector3.right * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : Vector3.right * motion.Strength);
          }

          break;
        case AxisEnum.Rot_y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(torque : Vector3.up * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : Vector3.up * motion.Strength);
          }

          break;
        case AxisEnum.Rot_z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(torque : Vector3.forward * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(torque : Vector3.forward * motion.Strength);
          }

          break;
        case AxisEnum.Dir_x_: break;
        case AxisEnum.Dir_y_: break;
        case AxisEnum.Dir_z_: break;
        default: throw new ArgumentOutOfRangeException();
      }

      this._fired_this_step = true;
    }
  }
}
