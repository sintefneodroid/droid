using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Actuators.WheelColliderActuator {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(ActuatorComponentMenuPath._ComponentMenuPath
                    + "WheelCollider/Torque"
                    + ActuatorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(WheelCollider))]
  public class TorqueActuator : Actuator {
    [SerializeField] WheelCollider _wheel_collider;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Torque"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override void Setup() { this._wheel_collider = this.GetComponent<WheelCollider>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void InnerApplyMotion(IMotion motion) {
      this._wheel_collider.motorTorque = motion.Strength;
    }

    public override string[] InnerMotionNames => new[] {"motorTorque"};
    void FixedUpdate() { ApplyLocalPositionToVisuals(this._wheel_collider); }

    /// <summary>
    /// </summary>
    static void ApplyLocalPositionToVisuals(WheelCollider col) {
      if (col.transform.childCount == 0) {
        return;
      }

      var visual_wheel = col.transform.GetChild(0);

      col.GetWorldPose(out var position, out var rotation);

      var transform1 = visual_wheel.transform;
      transform1.position = position;
      transform1.rotation = rotation;
    }
  }
}
