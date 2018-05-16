using System;
using UnityEngine;

namespace Neodroid.Prototyping.Motors.WheelColliderMotor {
  /// <summary>
  /// 
  /// </summary>
  [AddComponentMenu(
      MotorComponentMenuPath._ComponentMenuPath + "WheelCollider/Steering" + MotorComponentMenuPath._Postfix)]
  [RequireComponent(typeof(WheelCollider))]
  public class SteeringMotor : Motor {
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    WheelCollider _wheel_collider;

    /// <summary>
    /// 
    /// </summary>
    public override string PrototypingType { get { return "Steering"; } }

    /// <summary>
    /// 
    /// </summary>
    protected override void Setup() { this._wheel_collider = this.GetComponent<WheelCollider>(); }

    /// <summary>
    /// 
    /// </summary>
    void FixedUpdate() { ApplyLocalPositionToVisuals(this._wheel_collider); }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="motion"></param>
    protected override void InnerApplyMotion(Utilities.Messaging.Messages.MotorMotion motion) {
      this._wheel_collider.steerAngle = motion.Strength;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="col"></param>
    static void ApplyLocalPositionToVisuals(WheelCollider col) {
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
