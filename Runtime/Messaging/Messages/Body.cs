using UnityEngine;

namespace droid.Runtime.Messaging.Messages {
  /// <summary>
  /// </summary>
  public class Body {
    public Body(Vector3 vel, Vector3 ang) {
      this.Velocity = vel;
      this.AngularVelocity = ang;
    }

    /// <summary>
    /// </summary>
    public Vector3 Velocity { get; }

    /// <summary>
    /// </summary>
    public Vector3 AngularVelocity { get; }
  }
}
