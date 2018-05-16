using System;
using UnityEngine;

namespace Neodroid.Prototyping.Motors.WheelColliderMotor {
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "WheelCollider/Torque" + MotorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(WheelCollider))]
  public class TorqueMotor : Motor {
    [SerializeField] WheelCollider _wheel_collider;

    public override string PrototypingType { get { return "Torque"; } }

    protected override void Setup() { this._wheel_collider = this.GetComponent<WheelCollider>(); }

    protected override void InnerApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) {
      this._wheel_collider.motorTorque = motion.Strength;
    }

    void FixedUpdate() { this.ApplyLocalPositionToVisuals(this._wheel_collider); }

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
