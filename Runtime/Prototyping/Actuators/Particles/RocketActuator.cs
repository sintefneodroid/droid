using System;
using droid.Runtime.Enums;
using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators.Particles {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "Particles/Rocket"
                    + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(ParticleSystem))]
  [RequireComponent(typeof(Rigidbody))]
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
    public override string PrototypingTypeName { get { return "Rocket" + this._Axis_Of_Motion; } }

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
        Debug.Log($"It does not accept input {motion.Strength}, outside allowed range {this.MotionSpace.Min} to {this.MotionSpace.Max}");
        return; // Do nothing
      }

      switch (this._Axis_Of_Motion) {
        case Axis.X_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.right * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.right * motion.Strength);
          }

          break;
        case Axis.Y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.up * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.up * motion.Strength);
          }

          break;
        case Axis.Z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddForce(Vector3.forward * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeForce(Vector3.forward * motion.Strength);
          }

          break;
        case Axis.Rot_x_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.right * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.right * motion.Strength);
          }

          break;
        case Axis.Rot_y_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.up * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.up * motion.Strength);
          }

          break;
        case Axis.Rot_z_:
          if (this._Relative_To == Space.World) {
            this._Rigidbody.AddTorque(Vector3.forward * motion.Strength);
          } else {
            this._Rigidbody.AddRelativeTorque(Vector3.forward * motion.Strength);
          }

          break;
        case Axis.Dir_x_: break;
        case Axis.Dir_y_: break;
        case Axis.Dir_z_: break;
        default: throw new ArgumentOutOfRangeException();
      }

      this._fired_this_step = true;
    }
  }
}
