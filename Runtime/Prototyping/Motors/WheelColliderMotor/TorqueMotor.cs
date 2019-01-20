using droid.Runtime.Interfaces;
using UnityEngine;

namespace droid.Runtime.Prototyping.Motors.WheelColliderMotor {
  /// <inheritdoc />
  /// <summary>
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "WheelCollider/Torque" + MotorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    [SerializeField] WheelCollider _wheel_collider;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    public override string PrototypingTypeName { get { return "Torque"; } }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void Setup() { this._wheel_collider = this.GetComponent<WheelCollider>(); }

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    protected override void InnerApplyMotion(IMotorMotion motion) {
      this._wheel_collider.motorTorque = motion.Strength;
    }

    void FixedUpdate() { this.ApplyLocalPositionToVisuals(this._wheel_collider); }

    /// <summary>
    /// </summary>
    void ApplyLocalPositionToVisuals(WheelCollider col) {
      if (col.transform.childCount == 0) {
        return;
      }

      var visual_wheel = col.transform.GetChild(0);

      Vector3 position;
      Quaternion rotation;
      col.GetWorldPose(out position, out rotation);

      visual_wheel.transform.position = position;
      visual_wheel.transform.rotation = rotation;
    }
  }
}
