using System;
using Neodroid.Runtime.Interfaces;

namespace Neodroid.Runtime.Messaging.Messages {
  /// <summary>
  ///   Has a possible direction given by the sign of the float in strength
  /// </summary>
  [Serializable]
  public class MotorMotion : IMotorMotion {
    public MotorMotion(string actor_name, string motor_name, float strength) {
      this.ActorName = actor_name;
      this.MotorName = motor_name;
      this.Strength = strength;
    }

    /// <summary>
    /// </summary>
    public float Strength { get; set; }

    /// <summary>
    /// </summary>
    public string ActorName { get; }

    /// <summary>
    /// </summary>
    public string MotorName { get; }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    public override string ToString() {
      return "<MotorMotion> "
             + this.ActorName
             + ", "
             + this.MotorName
             + ", "
             + this.Strength
             + " </MotorMotion>";
    }
  }
}
